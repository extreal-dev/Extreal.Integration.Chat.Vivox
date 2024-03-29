﻿using System;
using Cysharp.Threading.Tasks;
using Extreal.Core.Common.System;
using Extreal.Integration.Chat.Vivox.MVS.App;
using UniRx;

namespace Extreal.Integration.Chat.Vivox.MVS.ChatControl
{
    public class ChatControlModel : DisposableBase
    {
        private readonly VivoxClient vivoxClient;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public ChatControlModel(VivoxClient vivoxClient)
            => this.vivoxClient = vivoxClient;

        public void Initialize()
            => vivoxClient.OnLoggedIn
                .Subscribe(_ =>
                {
                    var vivoxChannelConfig = new VivoxChannelConfig("GuestChannel");
                    vivoxClient.ConnectAsync(vivoxChannelConfig).Forget();
                })
                .AddTo(disposables);

        protected override void ReleaseManagedResources() => disposables.Dispose();

        public void OnStageTransitioned(StageName stageName)
        {
            if (AppUtils.IsSpace(stageName))
            {
                var vivoxAuthConfig = new VivoxAuthConfig("Guest");
                vivoxClient.LoginAsync(vivoxAuthConfig).Forget();
            }
            else
            {
                vivoxClient.DisconnectAllChannels();
                vivoxClient.Logout();
            }
        }
    }
}
