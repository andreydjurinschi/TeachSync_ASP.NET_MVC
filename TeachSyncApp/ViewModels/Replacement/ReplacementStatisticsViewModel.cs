using TeachSyncApp.Models;
namespace TeachSyncApp.ViewModels.Replacement;

public class ReplacementStatisticsViewModel
{
    public List<Models.Replacement> PendingReplacements { get; set; } = new List<Models.Replacement>();
    public List<ReplacementResponse> RejectedReplacements { get; set; } = new List<ReplacementResponse>();
    public List<ReplacementResponse> AppliedReplacements { get; set; } = new List<ReplacementResponse>();
}