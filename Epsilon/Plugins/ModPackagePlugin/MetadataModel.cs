using Stylet;
using System;
using TagTool.Cache;

namespace ModPackagePlugin
{
    class MetadataModel : PropertyChangedBase
    {
        private ModPackageMetadata _metadata;
        private ModPackageHeader _header;

        private bool _mainmenu;
        private bool _multiplayer;
        private bool _campaign;
        private bool _firefight;

        private bool _hasCustomizableCharacters;

        public MetadataModel(ModPackageMetadata metadata, ModPackageHeader header)
        {
            _metadata = metadata;
            _header = header;

            GetFlagValues();
        }

        private void GetFlagValues()
        {
            _mainmenu = _header.ModifierFlags.HasFlag(ModifierFlags.mainmenu);
            _multiplayer = _header.ModifierFlags.HasFlag(ModifierFlags.multiplayer);
            _campaign = _header.ModifierFlags.HasFlag(ModifierFlags.campaign);
            _firefight = _header.ModifierFlags.HasFlag(ModifierFlags.firefight);
            _hasCustomizableCharacters = _header.ModifierFlags.HasFlag(ModifierFlags.character);
        }

        public string Name
        {
            get => _metadata.Name;
            set
            {
                _metadata.Name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Author
        {
            get => _metadata.Author;
            set
            {
                _metadata.Author = value;
                NotifyOfPropertyChange();
            }
        }

        public string Description
        {
            get => _metadata.Description;
            set
            {
                _metadata.Description = value;
                NotifyOfPropertyChange();
            }
        }

        public string Website
        {
            get => _metadata.URL;
            set
            {
                _metadata.URL = value;
                NotifyOfPropertyChange();
            }
        }

        public short VersionMajor
        {
            get => _metadata.VersionMajor;
            set
            {
                _metadata.VersionMajor = value;
                NotifyOfPropertyChange();
            }
        }

        public short VersionMinor
        {
            get => _metadata.VersionMinor;
            set
            {
                _metadata.VersionMinor = value;
                NotifyOfPropertyChange();
            }
        }

        public bool Multiplayer
        {
            get => _multiplayer;
            set
            {
                _multiplayer = value;
                NotifyOfPropertyChange();
                ToggleFlag(ModifierFlags.multiplayer);
            }
        }

        public bool Campaign
        {
            get => _campaign;
            set
            {
                _campaign = value;
                NotifyOfPropertyChange();
                ToggleFlag(ModifierFlags.campaign);
            }
        }

        public bool MainMenu
        {
            get => _mainmenu;
            set
            {
                _mainmenu = value;
                NotifyOfPropertyChange();
                ToggleFlag(ModifierFlags.mainmenu);
            }
        }

        public bool Firefight
        {
            get => _firefight;
            set
            {
                _firefight = value;
                NotifyOfPropertyChange();
                ToggleFlag(ModifierFlags.firefight);
            }
        }

        public bool Character
        {
            get => _hasCustomizableCharacters;
            set
            {
                _hasCustomizableCharacters = value;
                NotifyOfPropertyChange();
                ToggleFlag(ModifierFlags.character);
            }
        }

        private void ToggleFlag(ModifierFlags flag)
        {
            if (_header.ModifierFlags.HasFlag(flag))
                _header.ModifierFlags &= ~flag;
            else
                _header.ModifierFlags |= flag;
        }
    }
}
