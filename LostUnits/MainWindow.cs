using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using PredefinedTypes = Predefined.PredefinedData;
using PluginInterface;

namespace LostUnits
{
    public partial class MainWindow : Form
    {
        public PredefinedTypes.Map Map { get; set; }
        public PredefinedTypes.Gameinformation Gameinfo { get; set; }
        public PredefinedTypes.PList Players { get; set; }
        public List<PredefinedTypes.Unit> Units { get; set; }
        public PredefinedTypes.LSelection Selection { get; set; }
        public List<PredefinedTypes.Groups> Groups { get; set; }

        private List<PlayerUnits> _players = new List<PlayerUnits>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private List<PlayerUnits> RebuildPlayerData()
        {
            if (Players == null || Units == null || Players.Count == 0 || Units.Count == 0)
            {
                return new List<PlayerUnits>();
            }

            var players = Players.Select(player => new PlayerUnits
            {
                AccountId = player.AccountId,
                Apm = player.Apm,
                ApmAverage = player.ApmAverage,
                ArmySupply = player.ArmySupply,
                CameraAngle = player.CameraAngle,
                CameraDistance = player.CameraDistance,
                CameraPositionX = player.CameraPositionX,
                CameraPositionY = player.CameraPositionY,
                CameraRotation = player.CameraRotation,
                ClanTag = player.ClanTag,
                Color = player.Color,
                CurrentBuildings = player.CurrentBuildings,
                Difficulty = player.Difficulty,
                Epm = player.Epm,
                EpmAverage = player.EpmAverage,
                Gas = player.Gas,
                GasArmy = player.GasArmy,
                GasIncome = player.GasIncome,
                IsLocalplayer = player.IsLocalplayer,
                Localplayer = player.Localplayer,
                Minerals = player.Minerals,
                MineralsArmy = player.MineralsArmy,
                MineralsIncome = player.MineralsIncome,
                Name = player.Name,
                NameLength = player.NameLength,
                PlayerRace = player.PlayerRace,
                Status = player.Status,
                SupplyMax = player.SupplyMax,
                SupplyMaxRaw = player.SupplyMaxRaw,
                SupplyMin = player.SupplyMin,
                SupplyMinRaw = player.SupplyMinRaw,
                Team = player.Team,
                Type = player.Type,
                ValidSize = player.ValidSize,
                Worker = player.Worker,
                Units = new List<PredefinedTypes.Unit>()
            }).ToList();

            foreach (var unit in Units)
            {
                if (unit.Owner >= 0 && unit.Owner < players.Count)
                {
                    players[unit.Owner].Units.Add(unit);
                }
            }

            return players;
        }

        private void tmrMainTimer_Tick(object sender, EventArgs e)
        {
            _players = RebuildPlayerData();
        }
    }

    public class PlayerUnits : PredefinedTypes.PlayerStruct
    {
        public List<PredefinedTypes.Unit> Units { get; set; }
    }

    public class AnotherSc2HackPlugin : IPlugins
    {
        private MainWindow _window;

        public string GetPluginDescription()
        {
            return "Lists all lost units";
        }

        public string GetPluginName()
        {
            return "LostUnits";
        }

        public bool GetRequiresGameinfo()
        {
            return true;
        }

        public bool GetRequiresGroups()
        {
            return false;
        }

        public bool GetRequiresMap()
        {
            return false;
        }

        public bool GetRequiresPlayer()
        {
            return true;
        }

        public bool GetRequiresSelection()
        {
            return false;
        }

        public bool GetRequiresUnit()
        {
            return true;
        }

        public void SetGameinfo(PredefinedTypes.Gameinformation gameinfo)
        {
            if (_window != null)
            {
                _window.Gameinfo = gameinfo;
            }
        }

        public void SetGroups(List<PredefinedTypes.Groups> groups)
        {
            if (_window != null)
            {
                _window.Groups = groups;
            }
        }

        public void SetMap(PredefinedTypes.Map map)
        {
            if (_window != null)
            {
                _window.Map = map;
            }
        }

        public void SetPlayers(PredefinedTypes.PList players)
        {
            if (_window != null)
            {
                _window.Players = players;
            }
        }

        public void SetSelection(PredefinedTypes.LSelection selection)
        {
            if (_window != null)
            {
                _window.Selection = selection;
            }
        }

        public void SetUnits(List<PredefinedTypes.Unit> units)
        {
            if (_window != null)
            {
                _window.Units = units;
            }
        }

        public void StartPlugin()
        {
            if (_window != null && !_window.IsDisposed)
            {
                _window.Close();
            }

            _window = new MainWindow();
            _window.Show();
        }

        public void StopPlugin()
        {
            if (_window != null && !_window.IsDisposed)
            {
                _window.Close();
            }
        }
    }
}
