﻿using System;
using Extreal.Core.Common.System;
using Extreal.Core.StageNavigation;
using Extreal.Integration.Chat.Vivox.MVS.App;
using UniRx;
using VContainer.Unity;

namespace Extreal.Integration.Chat.Vivox.MVS.ChatControl
{
    public class ChatControlPresenter : DisposableBase, IInitializable
    {
        private readonly StageNavigator<StageName, SceneName> stageNavigator;
        private readonly ChatControlModel chatControlModel;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public ChatControlPresenter
        (
            StageNavigator<StageName, SceneName> stageNavigator,
            ChatControlModel chatControlModel
        )
        {
            this.stageNavigator = stageNavigator;
            this.chatControlModel = chatControlModel;
        }

        public void Initialize()
        {
            chatControlModel.Initialize();
            stageNavigator.OnStageTransitioned
                .Subscribe(chatControlModel.OnStageTransitioned)
                .AddTo(disposables);
        }

        protected override void ReleaseManagedResources() => disposables.Dispose();
    }
}
