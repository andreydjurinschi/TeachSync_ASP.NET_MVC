using TeachSyncApp.Models;
namespace TeachSyncApp.ViewModels.Replacement;

public class ReplacementStatisticsViewModel
{
    public List<Models.Replacement> PendingReplacements { get; set; } = new List<Models.Replacement>();
    public List<Models.Replacement> RejectedReplacements { get; set; } = new List<Models.Replacement>();
    public List<Models.Replacement> AppliedReplacements { get; set; } = new List<Models.Replacement>();
}