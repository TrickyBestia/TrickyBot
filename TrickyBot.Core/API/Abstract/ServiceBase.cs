﻿// -----------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="TrickyBot Team">
// Copyright (c) TrickyBot Team. All rights reserved.
// Licensed under the CC BY-ND 4.0 license.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TrickyBot.API.Features;
using TrickyBot.API.Interfaces;
using TrickyBot.Services.ConsoleCommandService.API.Features;
using TrickyBot.Services.ConsoleCommandService.API.Interfaces;
using TrickyBot.Services.DiscordCommandService.API.Features;
using TrickyBot.Services.DiscordCommandService.API.Interfaces;

namespace TrickyBot.API.Abstract
{
    /// <summary>
    /// A service base class.
    /// </summary>
    /// <typeparam name="TConfig"><inheritdoc/></typeparam>
    public abstract class ServiceBase<TConfig> : IService<TConfig>
        where TConfig : IConfig, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase{TConfig}"/> class.
        /// </summary>
        public ServiceBase()
        {
            this.Config = new TConfig();
            this.DiscordCommands = DiscordCommandLoader.GetCommands(this.GetType().Assembly);
            this.ConsoleCommands = ConsoleCommandLoader.GetCommands(this.GetType().Assembly);
        }

        /// <inheritdoc/>
        public TConfig Config { get; internal set; }

        /// <inheritdoc/>
        public virtual IReadOnlyList<IDiscordCommand> DiscordCommands { get; }

        /// <inheritdoc/>
        public virtual IReadOnlyList<IConsoleCommand> ConsoleCommands { get; }

        /// <inheritdoc/>
        public abstract ServiceInfo Info { get; }

        /// <inheritdoc/>
        public async Task StartAsync()
        {
            Log.Info(this, $"Starting service \"{this.Info.Name}\" v{this.Info.Version.ToString(3)} by \"{this.Info.Author}\"...");
            try
            {
                await this.OnStart();
            }
            catch (Exception ex)
            {
                Log.Error(this, $"Exception thrown while starting service \"{this.Info.Name}\" v{this.Info.Version.ToString(3)} by \"{this.Info.Author}\": {ex}");
            }

            Log.Info(this, $"Service \"{this.Info.Name}\" v{this.Info.Version.ToString(3)} by \"{this.Info.Author}\" started.");
        }

        /// <inheritdoc/>
        public async Task StopAsync()
        {
            Log.Info(this, $"Stopping service \"{this.Info.Name}\" v{this.Info.Version.ToString(3)} by \"{this.Info.Author}\"...");
            try
            {
                await this.OnStop();
            }
            catch (Exception ex)
            {
                Log.Error(this, $"Exception thrown while stopping service \"{this.Info.Name}\" v{this.Info.Version.ToString(3)} by \"{this.Info.Author}\": {ex}");
            }

            Log.Info(this, $"Service \"{this.Info.Name}\" v{this.Info.Version.ToString(3)} by \"{this.Info.Author}\" stopped.");
        }

        /// <summary>
        /// A method that contains code that executes when the service starts.
        /// </summary>
        /// <returns>A task that represents the asynchronous start operation.</returns>
        protected abstract Task OnStart();

        /// <summary>
        /// A method that contains code that executes when the service stops.
        /// </summary>
        /// <returns>A task that represents the asynchronous stop operation.</returns>
        protected abstract Task OnStop();
    }
}