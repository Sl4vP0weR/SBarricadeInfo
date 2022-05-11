using System.Linq.Expressions;

namespace SBarricadeInfo;

public sealed partial class Main : RocketPlugin<Config>
{
    protected override void Unload()
    {
        UnturnedPlayerEvents.OnPlayerUpdateGesture -= GestureUpdatedHandler;
    }
    protected override void Load()
    {
        UnturnedPlayerEvents.OnPlayerUpdateGesture += GestureUpdatedHandler;
    }

    static async void GestureUpdatedHandler(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
    {
        // only when player is Pointing
        if (gesture != UnturnedPlayerEvents.PlayerGesture.Point)
            return;
        // checking for access
        if (!player.HasPermission(conf.Permission))
            return;
        // searching for barricade
        var aim = player.Player.look.aim;
        var raycastInfo = DamageTool.raycast(new(aim.position, aim.forward), 5f, RayMasks.BARRICADE);
        // validating barricade
        var hitTransform = raycastInfo.transform;
        if (!hitTransform)
            return;
        var barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(hitTransform);
        if (barricadeDrop is null)
            return;
        // getting all info that we need
        var barricadeAsset = barricadeDrop.asset;
        var barricadeData = barricadeDrop.GetServersideData();
        var barricade = barricadeData.barricade;
        var barricadeOwnerId = new CSteamID(barricadeData.owner);
        // sending formatted info
        UnturnedChat.Say(player, TranslateInfo(barricadeAsset.itemName, barricadeAsset.id, barricade.health, TryGetOwnerName(barricadeOwnerId), barricadeOwnerId));
    }
    public static string TryGetOwnerName(CSteamID id)
    {
        var target = PlayerTool.getPlayer(id);
        return target ? target.channel.owner.playerID.characterName : TranslateUnknown();
    }
}
