using ElementsTask.Presentation.BlockFieldCore.Views;

namespace ElementsTask.Presentation.BlockFieldCore.Extensions
{
    public static class BlockViewExtensions
    {
        public static bool IsNullOrEmpty(this BlockView blockView)
        {
            return blockView ==null || blockView.IsEmpty;
        }
    }
}