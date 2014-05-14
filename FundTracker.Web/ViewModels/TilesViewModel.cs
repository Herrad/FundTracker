using System.Collections.Generic;

namespace FundTracker.Web.ViewModels
{
    public class TilesViewModel
    {
        public IEnumerable<TileViewModel> Tiles { get; private set; }
        public string TileType { get; private set; }

        public TilesViewModel(IEnumerable<TileViewModel> tiles, string tileType)
        {
            TileType = tileType;
            Tiles = tiles;
        }
    }
}