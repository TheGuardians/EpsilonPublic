using CacheEditor;
using EpsilonLib.Commands;
using EpsilonLib.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using SimpleJSON;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ServerJsonEditor
{
    class ServerJsonEditorViewModel : Screen, INotifyPropertyChanged
    {
        private IShell _shell;
        private ICacheFile _cacheFile;
        private string _output;
        private string _serverPath;

        public string ServerFolder { get { return _serverPath; } set { _serverPath = value; } }
        public class MapEntry
        {
            public string DisplayName { get; set; }
            public string MapFileName { get; set; }
        }
        public class ModEntry : JSONNode
        {
            public string FileName { get; set; }
            public string DisplayName { get; set; }
            public string Link { get; set; }
        }
        public class TypeEntry : INotifyPropertyChanged
        {
            private string _typeName;
            private string _typeDisplayName;
            private int _randomChance;
            private ModEntry _modPackage;
            private ObservableCollection<ServerCommand> _commands;
            private ObservableCollection<CharacterOverride> _characterOverrides;
            private ObservableCollection<MapEntry> _specificMaps;

            public string TypeName { get => _typeName; set => _typeName = value; }
            public string TypeDisplayName { get => _typeDisplayName; set => _typeDisplayName = value; }
            public int RandomChance { get => _randomChance; set => _randomChance = value; }
            public ModEntry ModPackage { get => _modPackage; set => _modPackage = value; }
            public ObservableCollection<ServerCommand> Commands { get => _commands; set => _commands = value; }
            public ObservableCollection<CharacterOverride> CharacterOverrides { get => _characterOverrides; set => _characterOverrides = value; }
            public ObservableCollection<MapEntry> SpecificMaps { get => _specificMaps; set => _specificMaps = value; }

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public class ServerCommand : INotifyPropertyChanged
        {
            private string _name;
            private string _alias;
            private bool _enabled;
            public event PropertyChangedEventHandler PropertyChanged;

            public string Name { get => _name; set => _name = value; }
            public string Alias { get => _alias; set => _alias = value; }
            public bool Enabled
            {
                get => _enabled;
                set { _enabled = value; NotifyPropertyChanged("Commands"); }
            }

            private void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public class CharacterOverride : INotifyPropertyChanged
        {
            private string _team;
            private string _characterSet;
            private string _character;
            public event PropertyChangedEventHandler PropertyChanged;

            public string Team
            {
                get => _team;
                set { _team = value; NotifyPropertyChanged("Team"); }
            }

            public string CharacterSet
            {
                get => _characterSet;
                set { _characterSet = value; NotifyPropertyChanged("CharacterSet"); }
            }

            public string Character
            {
                get => _character;
                set { _character = value; NotifyPropertyChanged("Character"); }
            }

            private void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Dictionary<string, JSONNode> modsJsonDictionary;
        private JSONArray votingDefaultMapsArray;
        private JSONArray votingTypesArray;
        private ObservableCollection<string> _privateLocalMods;
        private ObservableCollection<string> _privateLocalGametypes;
        private ObservableCollection<ModEntry> _CurrentModList;
        private ObservableCollection<TypeEntry> _CurrentGametypeList;
        private ObservableCollection<ServerCommand> _CurrentEntryCommands;
        private ObservableCollection<CharacterOverride> _CurrentCharacterOverrides;
        private ObservableCollection<MapEntry> _CurrentSpecificMaps;
        private ObservableCollection<MapEntry> VotingDefaultMaps;

        private ObservableCollection<ServerCommand> CommandDefaults = new ObservableCollection<ServerCommand>()
        {
            new ServerCommand{ Name = "Server.SprintState", Alias="Sprint Allowed", Enabled = true },
            new ServerCommand{ Name = "Server.UnlimitedSprint", Alias="Unlimited Sprint", Enabled = false },
            new ServerCommand{ Name = "Server.EmotesEnabled", Alias="Emotes Allowed", Enabled = true },
            new ServerCommand{ Name = "Server.AssassinationEnabled", Alias="Assassinations", Enabled = true },
            new ServerCommand{ Name = "Server.NumberOfTeams", Alias="Multi-Team", Enabled = false },
            new ServerCommand{ Name = "Server.BottomlessClipEnabled", Alias="Bottomless Clip", Enabled = false },
            new ServerCommand{ Name = "Server.DualWieldEnabled", Alias="Dual Wielding Allowed", Enabled = true },
            new ServerCommand{ Name = "Server.HitMarkersEnabled", Alias="Hit Markers", Enabled = false },
            new ServerCommand{ Name = "Server.KillCommandEnabled", Alias="Kill Command", Enabled = false }
        };

        public Dictionary<ModEntry, ObservableCollection<TypeEntry>> modGametypeMapping =
            new Dictionary<ModEntry, ObservableCollection<TypeEntry>>();

        public string CacheFilePath => _cacheFile.File.FullName;

        public DelegateCommand SaveCommand { get; }

        public ObservableCollection<string> LocalModList {
            get { return _privateLocalMods; }
            set
            {
                _privateLocalMods = value;
                NotifyOfPropertyChange("LocalModList");
            }
        }
        public ObservableCollection<string> LocalMapList { get; set; }
        public ObservableCollection<string> LocalGametypeList
        {
            get { return _privateLocalGametypes; }
            set
            {
                _privateLocalGametypes = value;
                NotifyOfPropertyChange("CurrentModList");
            }
        }
        public ObservableCollection<ModEntry> CurrentModList
        {
            get { return _CurrentModList; }
            set
            {
                _CurrentModList = value;
                NotifyOfPropertyChange("CurrentModList");
            }
        }
        public ObservableCollection<TypeEntry> CurrentGametypeList
        {
            get { return _CurrentGametypeList; }
            set
            {
                _CurrentGametypeList = value;
                NotifyOfPropertyChange("CurrentGametypeList");
            }
        }
        public ObservableCollection<ServerCommand> CurrentEntryCommands
        {
            get { return _CurrentEntryCommands; }
            set
            {
                _CurrentEntryCommands = value;
                NotifyOfPropertyChange("CurrentEntryCommands");
            }
        }
        public ObservableCollection<CharacterOverride> CurrentCharacterOverrides
        {
            get { return _CurrentCharacterOverrides; }
            set
            {
                _CurrentCharacterOverrides = value;
                NotifyOfPropertyChange("CurrentCharacterOverrides");
            }
        }
        public ObservableCollection<MapEntry> CurrentSpecificMaps
        {
            get { return _CurrentSpecificMaps; }
            set
            {
                _CurrentSpecificMaps = value;
                NotifyOfPropertyChange("CurrentSpecificMaps");
            }
        }


        public ServerJsonEditorViewModel(IShell shell, ICacheFile cacheFile)
        {
            _shell = shell;
            _cacheFile = cacheFile;
            DisplayName = "Server Voting Editor";
            ServerFolder = Path.Combine(_cacheFile.File.Directory.Parent.FullName, @"data\server");

            GetServerCollections();
            GetPathCollections();
            
            SaveCommand = new DelegateCommand(Save, () => LocalGametypeList.Count() > 0);
        }

            // Initial Methods

        private ObservableCollection<string> AddDirectoryNames(DirectoryInfo directory)
        {
            ObservableCollection<string> itemList = new ObservableCollection<string>();
            int countTrim = 1;
            string criterion = "";

            switch (directory.Name)
            {
                case "map_variants":
                    criterion = "sandbox.map";
                    break;
                case "game_variants":
                    criterion = "variant.";
                    break;
                case "mods":
                    criterion = ".pak";
                    break;
            }

            foreach (var file in directory.GetFiles(".", SearchOption.AllDirectories).Where(s => s.ToString().Contains(criterion)))
            {
                string[] dirSplit = file.Directory.ToString().Split('\\');
                string name = dirSplit[dirSplit.Count() - countTrim];

                if (directory.Name == "mods")
                    name = file.Name.Split('.')[0];

                itemList.Add(name);
            }

            return itemList;
        }

        public object ParseJsonInitial(string path)
        {
            string contents;
            try
            {
                contents = File.ReadAllText(path);
                //StreamReader sr = new StreamReader(path);
                //contents = sr.ReadToEnd();
                //sr.Close();
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is DirectoryNotFoundException)
                {
                    var alert = new AlertDialogViewModel
                    {
                        AlertType = Alert.Error,
                        DisplayName = "File Not Found",
                        Message = $"JSON could not be found at the following path:",
                        SubMessage = $"{path}"
                    };
                    _shell.ShowDialog(alert);
                }
                else
                {
                    var alert = new AlertDialogViewModel
                    {
                        AlertType = Alert.Error,
                        Message = $"JSON at following path could not be parsed:",
                        SubMessage = $"{path}"
                    };
                    _shell.ShowDialog(alert);
                }
                return null;
            };

            var json = JSON.Parse(contents);
            return json;
        }

        private void GetServerCollections()
        {
            var serverDirectory = new DirectoryInfo(ServerFolder);
            if (serverDirectory.Exists)
            {
                try
				{
                    JSONNode mods;
                    var path = "\\mods.json";

                    if (!File.Exists(ServerFolder + path))
                        path += ".example";

                    mods = ((JSONClass)ParseJsonInitial(serverDirectory + path))["mods"];
                    modsJsonDictionary = ((JSONClass)mods).ToDictionary();
                }
				catch
				{
                    var alert = new AlertDialogViewModel
                    {
                        AlertType = Alert.Error,
                        DisplayName = "Error in mods.json",
                        Message = "mods.json has a formatting error and could not be parsed.",
                        SubMessage = ServerFolder + "\\mods.json"
                    };
                    _shell.ShowDialog(alert);
                }

                try
				{
                    var path = "\\voting.json";
                    if (!File.Exists(ServerFolder + path))
                        path += ".example";

                    votingDefaultMapsArray = ((JSONClass)ParseJsonInitial(serverDirectory + path))["maps"].AsArray;
                    votingTypesArray = ((JSONClass)ParseJsonInitial(serverDirectory + path))["types"].AsArray;
                }
                catch
				{
                    var alert = new AlertDialogViewModel
                    {
                        AlertType = Alert.Error,
                        DisplayName = "Error in voting.json",
                        Message = $"voting.json has a formatting error and could not be parsed.",
                        SubMessage = ServerFolder + "\\voting.json"
                    };
                    _shell.ShowDialog(alert);
                }
            }
            else
            {
                var alert = new AlertDialogViewModel
                {
                    AlertType = Alert.Error,
                    Message = $"Server directory at the following path could not be found:",
                    SubMessage = $"{serverDirectory}"
                };
                _shell.ShowDialog(alert);
            }

            modGametypeMapping = CreateModEntryCollection(modsJsonDictionary, votingTypesArray);
            VotingDefaultMaps = CreateMapEntryCollection(votingDefaultMapsArray);

            //votingMapList.CollectionChanged += Files_CollectionChanged;
        }
        private void GetPathCollections()
        {
            string fullPath = _cacheFile.File.Directory.FullName;

            // Gather Available Map Names

            var mapDirectory = new DirectoryInfo(Path.Combine(fullPath, "..\\data\\map_variants"));
            if (mapDirectory.Exists)
                LocalMapList = AddDirectoryNames(mapDirectory);

            // Gather Available Variant Names

            var variantDirectory = new DirectoryInfo(Path.Combine(fullPath, "..\\data\\game_variants"));
            if (variantDirectory.Exists)
                LocalGametypeList = AddDirectoryNames(variantDirectory);

            if (!variantDirectory.Exists || LocalGametypeList.Count() == 0)
            {
                var alert = new AlertDialogViewModel
                {
                    AlertType = Alert.Standard,
                    DisplayName = "No Gametypes Found",
                    Message = $"Couldn't find any game variants saved to your \"data\\game_variants\" folder. Download or create some gametypes!"
                };
                _shell.ShowDialog(alert);
            }

            // Gather Available Pak Names

            var pakDirectory = new DirectoryInfo(Path.Combine(fullPath, "..\\mods"));
            if (pakDirectory.Exists)
                LocalModList = AddDirectoryNames(pakDirectory);
            if (!pakDirectory.Exists || LocalModList.Count() == 0)
            {
                var alert = new AlertDialogViewModel
                {
                    AlertType = Alert.Standard,
                    DisplayName = "No Mod Packages Found",
                    Message = $"Couldn't find any mod packages in your \"mods\" folder. Download or create some mods!"
                };
                _shell.ShowDialog(alert);
            }

            CurrentModList = new ObservableCollection<ModEntry>(modGametypeMapping.Keys.ToList());
            NotifyOfPropertyChange("CurrentModList");
            UpdateAvailablePaks(null);
        }
        private ObservableCollection<MapEntry> CreateMapEntryCollection(JSONArray mapNodes)
        {
            ObservableCollection<MapEntry> mapCollection = new ObservableCollection<MapEntry>();

            foreach (JSONNode mapElem in mapNodes)
            {
                mapCollection.Add(new MapEntry
                {
                    DisplayName = mapElem["displayName"],
                    MapFileName = mapElem["mapName"],
                });
            }
            return mapCollection;
        }
        private Dictionary<ModEntry, ObservableCollection<TypeEntry>> CreateModEntryCollection(Dictionary<string, JSONNode> modNodes, JSONArray typeNodes)
        {
            Dictionary<ModEntry, ObservableCollection<TypeEntry>> modTypeDictionary = new Dictionary<ModEntry, ObservableCollection<TypeEntry>>();

            // create mod collection   

            ModEntry none = new ModEntry
            {
                FileName = "<none>",
                DisplayName = "<none>",
                Link = "<none>"
            };

            modTypeDictionary.Add(none, new ObservableCollection<TypeEntry>());

            foreach (KeyValuePair<string, JSONNode> N in modNodes)
            {
                modTypeDictionary.Add(new ModEntry
                {
                    FileName = N.Key.ToString().Replace("\"", ""),
                    DisplayName = N.Key.ToString().Replace("\"", ""),
                    Link = N.Value["package_url"].ToString().Replace("\"", "")
                },
                new ObservableCollection<TypeEntry>());
            }

            // map each voting entry to a mod in the collection

            foreach (JSONNode typeElem in typeNodes)
            {
                int dupeAmount = -1;
                int.TryParse(typeElem["randomChance"].Value.ToString(), out dupeAmount);

                for (int i = 0; i < modTypeDictionary.Count; i++)
                {
                    ModEntry mod = null;
                    string modKeyName = (modTypeDictionary.ElementAt(i).Key).FileName;
                    string entryModName = typeElem["modPack"].ToString().Replace("\"", "");

                    if (entryModName == "" && modKeyName == "<none>")
                    {
                        entryModName = "<none>";
                        mod = none;
                    }

                    if (entryModName == modKeyName)
                    {
                        mod = modTypeDictionary.ElementAt(i).Key;

                        if (mod.DisplayName != typeElem["modDisplayName"].ToString())
                            modTypeDictionary.ElementAt(i).Key.DisplayName = typeElem["modDisplayName"].ToString().Replace("\"", "");

                        modTypeDictionary.ElementAt(i).Value.Add(new TypeEntry
                        {
                            TypeName = typeElem["typeName"].ToString().Replace("\"", ""),
                            TypeDisplayName = typeElem["displayName"].ToString().Replace("\"", ""),
                            RandomChance = (dupeAmount == 0) ? 1 : dupeAmount,
                            ModPackage = mod,
                            Commands = GetCommands(typeElem["commands"]),
                            SpecificMaps = CreateMapEntryCollection(typeElem["specificMaps"].AsArray),
                            CharacterOverrides = GetCharacterOverrides(typeElem["characterOverrides"] as JSONClass)
                        });
                    }
                }
            }

            return modTypeDictionary;
        }

        public ObservableCollection<ServerCommand> GetCommands(JSONNode node)
        {
            string[] cmdList = node.ToString().Trim(new char[] { '{', '}', '[', ']' }).Split(',');
            cmdList = cmdList.Select(x => x.Replace("\"", "").Trim()).ToArray();

            var commandList = new ObservableCollection<ServerCommand>();
            foreach (ServerCommand command in CommandDefaults)
                commandList.Add(command.DeepClone());

            if (cmdList.Count() > 0 && !string.IsNullOrEmpty(cmdList[0]))
            {
                foreach (string cmd in cmdList)
                {
                    string[] pair = cmd.Split(' ');
                    bool enabled = false;

                    if (pair[0] == "Server.NumberOfTeams")
					{
                        int.TryParse(pair[1], out int result);

                        if (result > 2)
                            enabled = true;
                        else
                            enabled = false;
                    }
                    else if (pair[1] != "0")
                        enabled = true;

                    foreach (ServerCommand command in commandList)
                        if (command.Name == pair[0])
                            command.Enabled = enabled;
                }
            }
            return commandList;
        }

        public ObservableCollection<CharacterOverride> GetCharacterOverrides(JSONClass overrideNodes)
        {
            ObservableCollection<CharacterOverride> overrides = new ObservableCollection<CharacterOverride>() { };

            if (overrideNodes != null)
			{
                var overrideDict = overrideNodes.ToDictionary();

                foreach (var key in overrideDict.Keys)
                {
                    var charVal = overrideDict[key].AsArray;

                    overrides.Add(new CharacterOverride
                    {
                        Team = key.ToString(),
                        CharacterSet = charVal[0].ToString().Replace("\"", ""),
                        Character = charVal[1].ToString().Replace("\"", "")
                    });
                }
            }

            return overrides;
        }

        // Modification Methods


        public void UpdateAvailablePaks(string pakFileName)
        {
            if (pakFileName == null)
            {
                foreach (ModEntry key in modGametypeMapping.Keys)
                {
                    if (LocalModList.IndexOf(key.FileName) != -1)
                    {
                        int x = LocalModList.IndexOf(key.FileName);
                        LocalModList.RemoveAt(x);
                    }
                }
            }
            else
            {
                //int i = LocalModList.IndexOf(pakFileName);
                if (LocalModList.Contains(pakFileName))
                    LocalModList.Remove(pakFileName);
                else
                    LocalModList.Add(pakFileName);

                //if (i == -1)
                //    LocalModList.Add(pakFileName);
                //else
                //    LocalModList.RemoveAt(i);

                NotifyOfPropertyChange("LocalModList");
            }
        }

            // Addition/Removal Methods

        public void AddMod(string pakFileName)
        {
            ModEntry newEntry = new ModEntry()
            {
                FileName = pakFileName,
                DisplayName = pakFileName,
                Link = ""
            };

            modGametypeMapping.Add(newEntry, new ObservableCollection<TypeEntry>());
            CurrentModList.Add(newEntry);

            NotifyOfPropertyChange("CurrentModList");
            UpdateAvailablePaks(pakFileName);
        }
        public void RemoveMod(ModEntry modToRemove)
        {
            modGametypeMapping.Remove(modToRemove);
            CurrentModList.Remove(modToRemove);

            NotifyOfPropertyChange("CurrentModList");
            UpdateAvailablePaks(modToRemove.FileName);
        }

        public void AddGametype(string gameVariantName, ModEntry currentMod)
        {
            var freshCommands = new ObservableCollection<ServerCommand>();
            foreach (ServerCommand command in CommandDefaults)
                freshCommands.Add(command.DeepClone());

            TypeEntry newEntry = new TypeEntry()
            {
                TypeName = gameVariantName,
                TypeDisplayName = "",
                ModPackage = currentMod,
                RandomChance = 1,
                Commands = freshCommands,
                CharacterOverrides = new ObservableCollection<CharacterOverride>(),
                SpecificMaps = new ObservableCollection<MapEntry>()
            };

            //gametypeMapping.Add(newEntry, new ObservableCollection<TypeEntry>());
            CurrentGametypeList.Add(newEntry);
            NotifyOfPropertyChange("CurrentGametypeList");
        }
        public void RemoveGametype(TypeEntry typeToRemove)
        {
            CurrentGametypeList.Remove(typeToRemove);
            NotifyOfPropertyChange("CurrentGametypeList");
        }

        public void AddMap(string mapNameToAdd)
		{
            MapEntry newEntry = new MapEntry()
            {
                MapFileName = mapNameToAdd,
                DisplayName = mapNameToAdd
            };

            CurrentSpecificMaps.Add(newEntry);
            NotifyOfPropertyChange("CurrentSpecificMaps");
		}

            // Misc

        private void Files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        private async Task CreateBackupAsync(string outputPath)
        {
            var zipArchive = new ZipArchive(File.Create(outputPath), ZipArchiveMode.Create);
            var zipEntries = new HashSet<string>();

            foreach (var filePath in LocalMapList)
            {
                var sandboxMapFile = new FileInfo(filePath);
                var relativePath = sandboxMapFile.FullName.Replace(sandboxMapFile.Directory.FullName.Replace(sandboxMapFile.Directory.Name, ""), "");
                if (!zipEntries.Add(relativePath))
                    throw new InvalidOperationException($"Could not create backup, conflicting folder names '${relativePath}'");

                var entry = zipArchive.CreateEntry(relativePath);
                using (var entryStream = entry.Open())
                {
                    using (var inputStream = sandboxMapFile.Open(FileMode.Open, FileAccess.Read))
                        await inputStream.CopyToAsync(entryStream);
                }
            }
            zipArchive.Dispose();
        }
        private string GetBackupFileName()
        {
            return $"map_variant_backup_{DateTime.Now.ToFileTime()}.zip";
        }
        private void WriteLog(string output)
        {
            Application.Current.Dispatcher.Invoke(() => Output += $"{output}\n");
        }
        public string Output
        {
            get => _output;
            set => SetAndNotify(ref _output, value);
        }

        public void ReloadAll()
		{
            GetServerCollections();
            GetPathCollections();
        }

        internal void Save()
		{
			var modsJson = PrepareModsJson();
            File.WriteAllText(ServerFolder + "\\mods.json", modsJson);
            
            var votingString = PrepareVotingJson();
            File.WriteAllText(ServerFolder + "\\voting.json", votingString);

            var alert = new AlertDialogViewModel
            {
                AlertType = Alert.Success,
                Message = $"\"mods.json\" and \"voting.json\" have been saved to the following directory:",
                SubMessage = $"{ServerFolder}"
            };
            _shell.ShowDialog(alert);
        }

        private string PrepareModsJson()
		{
			var modsTemp = new JSONClass();

			foreach (var mod in modGametypeMapping.Keys)
			{
				var urlNode = new JSONClass
				{
					{ "package_url", mod.Link }
				};

				if (mod.FileName != "<none>")
					modsTemp.Add(mod.FileName, urlNode);
			}

			modsTemp = new JSONClass
			{
				{ "mods", modsTemp }
			};

			var modString = modsTemp.ToString();
			modString = JToken.Parse(modString).ToString(Formatting.Indented);

            return modString;
		}
        
        private string PrepareVotingJson()
		{
			var voting = new JSONClass();

			JSONArray defaultMapsArray = PrepareDefaultMapsArray();
			voting.Add("maps", defaultMapsArray);

			List<TypeEntry> allGametypes = new List<TypeEntry>();
			foreach (ObservableCollection<TypeEntry> typeCollection in modGametypeMapping.Values)
			{
				foreach (TypeEntry typeEntry in typeCollection)
				{
					allGametypes.Add(typeEntry);
				}
			}
			JSONArray gametypeArray = PrepareGametypeArray(allGametypes);
			voting.Add("types", gametypeArray);

			// Format and write

			var votingString = voting.ToString();
			votingString = JToken.Parse(votingString).ToString(Formatting.Indented);

			return votingString;
		}

		private JSONArray PrepareGametypeArray(List<TypeEntry> allGametypes)
		{
			var gametypeArray = new JSONArray();

			foreach (TypeEntry type in allGametypes)
			{
				JSONClass typeValues = new JSONClass();

				JSONArray commands = PrepareCommands(type);
				if (commands.Count > 0)
					typeValues.Add("commands", commands);

				typeValues.Add("typeName", type.TypeName);

				if (!string.IsNullOrEmpty(type.TypeDisplayName))
					typeValues.Add("displayName", type.TypeDisplayName);

                if (type.RandomChance > 1)
				{
                    typeValues.Add("randomChance", type.RandomChance.ToString());
                }

				if (type.ModPackage.FileName != "<none>")
				{
					typeValues.Add("modPack", type.ModPackage.FileName);

					string modDisplay = type.ModPackage.DisplayName;
					if (!string.IsNullOrEmpty(modDisplay) && modDisplay != type.ModPackage.FileName)
						typeValues.Add("modDisplayName", type.ModPackage.DisplayName);
				}

				JSONClass charOverrides = PrepareCharOverrides(type);
				if (charOverrides.Count > 0)
					typeValues.Add("characterOverrides", charOverrides);

				JSONArray specificMaps = PrepareSpecificMaps(type);
				if (specificMaps.Count > 0)
					typeValues.Add("specificMaps", specificMaps);

				gametypeArray.Add(typeValues);
			}

			return gametypeArray;
		}

		private JSONArray PrepareCommands(TypeEntry type)
		{
            JSONArray commandArray = new JSONArray();

            foreach (ServerCommand currentCmd in type.Commands)
			{
                foreach (ServerCommand defaultCmd in CommandDefaults)
				{
                    if (currentCmd.Name == defaultCmd.Name && currentCmd.Enabled != defaultCmd.Enabled)
					{
                        string cmdAsString = currentCmd.Name + " ";

                        if (currentCmd.Name == "Server.NumberOfTeams" && currentCmd.Enabled == true)
                            cmdAsString += "4";
                        else if (currentCmd.Enabled == true)
                            cmdAsString += "1";
                        else if (currentCmd.Enabled == false)
                            cmdAsString += "0";

                        commandArray.Add(cmdAsString);
					}
				}
			}

			return commandArray;
		}

		private static JSONArray PrepareSpecificMaps(TypeEntry type)
		{
			JSONArray specificMaps = new JSONArray();
			foreach (var item in type.SpecificMaps)
			{
				JSONClass mapClass = new JSONClass();
				if (!string.IsNullOrEmpty(item.DisplayName) && !string.IsNullOrEmpty(item.MapFileName))
				{
					mapClass.Add("displayName", item.DisplayName);
					mapClass.Add("mapName", item.MapFileName);

					specificMaps.Add(mapClass);
				}
			}

			return specificMaps;
		}

		private static JSONClass PrepareCharOverrides(TypeEntry type)
		{
			JSONClass charOverrides = new JSONClass();
			foreach (var item in type.CharacterOverrides)
			{
				JSONArray overrideArray = new JSONArray();
				if (!string.IsNullOrEmpty(item.CharacterSet) && !string.IsNullOrEmpty(item.Character))
				{
					overrideArray.Add(item.CharacterSet);
					overrideArray.Add(item.Character);

					charOverrides.Add(item.Team, overrideArray);
				}
			}

			return charOverrides;
		}

		private JSONArray PrepareDefaultMapsArray()
		{
			var defaultMapsArray = new JSONArray();

			foreach (MapEntry map in VotingDefaultMaps)
			{
				var defaultMap = new JSONClass
				{
					{ "displayName", map.DisplayName },
					{ "mapName", map.MapFileName }
				};

				defaultMapsArray.Add(defaultMap);
			}

			return defaultMapsArray;
		}
	}
}
