namespace ServerCore.Models.Responses;

public class RewardsResponse
{
    public List<Reward> rewards { get; set; } = [];

    public class Reward
    {
        public Guid profileId { get; set; }
        public Guid rewardId { get; set; }
        public Guid spaceId { get; set; }
        public string status { get; set; } = string.Empty;
        public int purchaseQuantity { get; set; }
        public DateTime purchasedAt { get; set; }
        public int currentUnitsPrice { get; set; }
        public List<object> completedConditions { get; set; } = [];
        public List<string> limitReachedDetails { get; set; } = [];
        public List<object> lockedDetails { get; set; } = [];
        public object? ubiPlusCooldownEndsAt { get; set; } = null;
        public string source { get; set; } = "UbiConnect";
    }
}
