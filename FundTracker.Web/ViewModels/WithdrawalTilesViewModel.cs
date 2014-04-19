using System.Collections.Generic;

namespace FundTracker.Web.ViewModels
{
    public class WithdrawalTilesViewModel
    {
        public WithdrawalTilesViewModel(IEnumerable<WithdrawalTileViewModel> tiles)
        {
            Tiles = tiles;
        }

        public IEnumerable<WithdrawalTileViewModel> Tiles { get; private set; }
    }
}