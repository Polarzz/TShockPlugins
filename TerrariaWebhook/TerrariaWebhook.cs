using Terraria;
using System.Net;
using System.Text;
using TerrariaApi.Server;
using TShockAPI;

namespace TerrariaWebhook
{
    [ApiVersion(2, 1)]
    public class TerrariaWebhook : TerrariaPlugin
    {

        // config variables
        // change webhookUrl to the URL of your webhook
        // change defaultAvatar to the URL of an image file you would like the webhook to have
        public string defaultAvatar = "https://static.wikia.nocookie.net/terraria_gamepedia/images/5/5a/App_icon_1.3_Update.png/revision/latest?cb=20191202230012";
        public string webhookUrl = "WEBHOOK_HERE";

        public override string Name => "Terraria Webhook";
        public override Version Version => new Version(1, 0);
        public void Webhook(string message, string username, string pfp)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            string payload = "{\"content\": \"" + message + "\", \"username\": \"" + username + "\",\"avatar_url\":\"" + pfp + "\"}";
            client.UploadData(webhookUrl, Encoding.UTF8.GetBytes(payload));
        }

        public TerrariaWebhook(Main game) : base(game) { }

        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, Init);
        }

        private void OnChatReceived(ServerChatEventArgs arg)
        {
            Webhook("**" + TShock.Players[arg.Who].Name + "**: " + arg.Text, "Terraria", defaultAvatar);
        }

        private void OnJoin(GreetPlayerEventArgs player)
        {
            Webhook("**" + TShock.Players[player.Who].Name + "** joined.", "Terraria", defaultAvatar);
        }
        private void OnLeave(LeaveEventArgs player)
        {
            Webhook("**" + TShock.Players[player.Who].Name + "** left.", "Terraria", defaultAvatar);
        }
        private void Init(EventArgs args)
        {
            ServerApi.Hooks.ServerChat.Register(this, OnChatReceived);
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnJoin);
            ServerApi.Hooks.ServerLeave.Register(this, OnLeave);

        }

        protected override void Dispose(bool disposing) { if (disposing) { ServerApi.Hooks.GameInitialize.Register(this, Init); } base.Dispose(disposing); }



    }


}
