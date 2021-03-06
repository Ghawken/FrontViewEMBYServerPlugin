﻿using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Controller.Session;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Serialization;

using System;

using System.Linq;
using System.Threading;

namespace MediaBrowser.Plugins.FrontView
{
    public class ActionClass : IServerEntryPoint
    {
       // private readonly IJsonSerializer _json;
        private readonly ISessionManager _sessionManager;
        private readonly ILibraryManager _libraryManager;
        private readonly ILogger _logger;
      //  private readonly INetworkManager _networkManager;
        public Timer timer1;
        public static ActionClass Instance { get; private set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="sessionManager"> </param>
        /// <param name="userDataManager"></param>
        /// <param name="libraryManager"> </param>
        /// <param name="logger"></param>
        /// <param name="httpClient"></param>
        /// <param name="appHost"></param>
        /// <param name="fileSystem"></param>
        public ActionClass(IJsonSerializer jsonSerializer, ISessionManager sessionManager, IUserDataManager userDataManager, ILibraryManager libraryManager, ILogManager logger)
        {
            Instance = this;
            _sessionManager = sessionManager;
            _libraryManager = libraryManager;
            _logger = logger.GetLogger("FrontView+");


        }
        public void GetClientInfo(object sender, SessionEventArgs s)
        {

            var DevicesRunning = Plugin.Instance.Configuration.DevicesRunning;

            try
            {
                bool index = false;
                _logger.Debug("--- FrontView Plugin ---:::: Running Get Client Info");

                _logger.Debug("--- FrontView Plugin:  Checking if already in list of Devices");

                // Check if already in Index of Devices or Not
                // Issue if DevicesRunning has nothing in it - Error - Below to check first
                if (Plugin.Instance.Configuration.DevicesRunning != null)
                {
                    if (Plugin.Instance.Configuration.DevicesRunning.Count > 0)
                    {
                        index = Plugin.Instance.Configuration.DevicesRunning.Any(item => item.Id == s.SessionInfo.Id);
                    }
                }

                _logger.Debug("--- FrontView Plugin --::::: GetClientInfo:  Is Current ID UUID in List? ");

                if (index == false)
                {
                    _logger.Debug("---FrontView Plugin --::::   Add New Device to the Device List -- Shouldn't be running much");

                    string devicenull = string.IsNullOrEmpty(s.SessionInfo.DeviceName) ? "deviceNull" : s.SessionInfo.DeviceName+" "+s.SessionInfo.DeviceType;

                    _logger.Debug("--- FrontView Plugin:  Devicenull :" + devicenull);

                    // Change back to Device.Id here 3.4.1.27

                    string deviceidnull = string.IsNullOrEmpty(s.SessionInfo.UserName) ? "UserName Empty" : s.SessionInfo.UserName;
                   
                    _logger.Debug("--- FrontView Plugin:  DeviceIdnull :" + deviceidnull);


                    // change below to username - check where client is used.
                    //string clientnull = string.IsNullOrEmpty(s.SessionInfo.Client) ? "Client Unknown" : s.SessionInfo.Client;


                    string clientnull = string.IsNullOrEmpty(s.SessionInfo.Client) ? "Client Unknown" : s.SessionInfo.Client+" "+s.SessionInfo.ApplicationVersion;

                    _logger.Debug("--- FrontView Plugin:  Clientnull :" + clientnull);

                    string idnull = string.IsNullOrEmpty(s.SessionInfo.Id) ? "Id Unknown" : s.SessionInfo.Id;

                    _logger.Debug("--- FrontView Plugin:  IdNull :" + idnull);

                    DevicesRunning.Add(new Configuration.PluginConfiguration.DeviceInfoList() { DeviceName = devicenull.ToString(), DeviceId = deviceidnull.ToString(), Client = clientnull.ToString(), Id = idnull.ToString() });

                    //Plugin.Instance.Configuration.DevicesRunning.Add(new Configuration.PluginConfiguration.DeviceInfoList() { DeviceName = devicenull.ToString() , DeviceId = deviceidnull.ToString() , Client = clientnull.ToString() , Id = idnull.ToString() });
                }

                //newDevice.Add (new Configuration.PluginConfiguration.DeviceInfoList() { DeviceName=s.SessionInfo.DeviceName, DeviceId=s.SessionInfo.DeviceId})                         

                _logger.Debug("---- FrontView Plugin:  Now running update");

                Plugin.Instance.UpdateConfiguration(Plugin.Instance.Configuration);
            }
            catch (Exception ex)
            {
                _logger.Debug("--- FrontView Plugin:  Exception in GetClient Info..." + ex);
                Plugin.Instance.SaveConfiguration();
            }

        }
        public void Run()
        {
            //  _sessionManager.PlaybackStart += KernelPlaybackStart;
            //  _sessionManager.PlaybackStopped += KernelPlaybackStop;
            //  _sessionManager.PlaybackProgress += ;

           
            _sessionManager.SessionStarted += GetClientInfo;

   
            //_logger.Debug("--- YATSE ---:::: Checking this only runs once - not forever");
            //     MediaBrowser.Common.Events.EventHelper



            //  _libraryManager.ItemAdded += LibraryManagerItemAdded;
            //  _libraryManager.ItemRemoved += LibraryManagerItemRemoved;
        }
        /*

                public void PauseTimer()
                {
                    _logger.Debug("YATSE PAUSE TIMER RUN");
            

            
                    timer1 = new Timer();
                    timer1.Tick += new EventHandler( timer1_Tick);
                    timer1.Interval = 1000; // in miliseconds
                    timer1.Start();
                }


                public void timer1_Tick(object sender, EventArgs e)
                {
                    _logger.Debug("Yatse timer1_Tick Run");
                    CheckPaused(_sessionManager);
                }
                */
        public async void CheckPaused(object sender, PlaybackProgressEventArgs e)
        {

            _logger.Debug("+++++++++++++++++++++ CHECK PAUSED HAS BEEN RUN +++++++++++++++++++++++++++++++++++++");

            foreach (var data in _sessionManager.Sessions)
            {
                if (data.PlayState.IsPaused)
                {
                    _logger.Debug("******************** PAUSED ***********************");
                }
                if (data.PlayState.IsPaused == false)
                {
                    _logger.Debug("******************** NOT PAUSED ***********************");
                }
            }

            /* 
           if (s.SessionInfo.PlayState != null)
           {
               if (s.SessionInfo.PlayState.IsPaused)
               {
                   _logger.Debug("Yatse: Yah :Paused is True - Sorting out");
                   return;
               }
               else
               {
                   return;
               }
           }
           */
            return;

        }


        public async void KernelPlaybackStart(object sender, PlaybackProgressEventArgs e)
        {

            try
            {
                _logger.Debug("Yatse : Playback Started");
                _logger.Debug("Yatse: Device ID " + e.DeviceId);
                _logger.Debug("Yatse: Device Name " + e.DeviceName);
                _logger.Debug("Yatse: Item.Parent " + e.Item.Parent);
                _logger.Debug("Yatse: MediaInfo.Name " + e.MediaInfo.Name);
                _logger.Debug("Yatse: MediaInfo.MediaType " + e.MediaInfo.MediaType);
                _logger.Debug("Yatse: FileNamewithout Extension " + e.Item.FileNameWithoutExtension);
                _logger.Debug("Yatse: e.Item.path :  " + e.Item.Path);
              //  _logger.Debug("Yatse: MediaInfo.Physical Location " + e.Item.PhysicalLocations);
                _logger.Debug("Yatse: MediaSourceID " + e.MediaSourceId);
                if (e.PlaybackPositionTicks != null)
                {
                    _logger.Debug("Yatse: PlaybackPosition Ticks " + e.PlaybackPositionTicks.HasValue);
                }

                if (e.Users == null || !e.Users.Any() || e.Item == null)
                {
                    _logger.Error("Event details incomplete. Cannot process current media");
                    return;
                }

                var video = e.Item as Video;
                var progressPercent = video.RunTimeTicks.HasValue && video.RunTimeTicks != 0 ?
                    (float)(e.PlaybackPositionTicks ?? 0) / video.RunTimeTicks.Value * 100.0f : 0.0f;
                _logger.Debug("Yatse: PROGRESS PERECENT " + progressPercent);
                try
                {
                    if (video is Movie)
                    {
                        _logger.Debug("Send movie status update");
                    }
                    else if (video is Episode)
                    {
                        _logger.Debug("Send episode status update");
                    }
                }
                catch (Exception ex)
                {
                    _logger.ErrorException("Exception handled sending status update", ex);
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Error sending watching status update", ex, null);
            }


        }

        private async void KernelPlaybackStop(object sender, PlaybackProgressEventArgs e)
        {
            try
            {
                _logger.Debug("Yatse : Playback Stopped");
                _logger.Debug("Yatse: Device ID " + e.DeviceId);
                _logger.Debug("Yatse: Device Name " + e.DeviceName);
                _logger.Debug("Yatse: Item.Parent " + e.Item.Parent);
                _logger.Debug("Yatse: MediaInfo.Name " + e.MediaInfo.Name);
                _logger.Debug("Yatse: MediaInfo.MediaType " + e.MediaInfo.MediaType);
                _logger.Debug("Yatse: FileNamewithout Extension " + e.Item.FileNameWithoutExtension);
                _logger.Debug("Yatse: e.Item.path :  " + e.Item.Path);
              //  _logger.Debug("Yatse: MediaInfo.Physical Location " + e.Item.PhysicalLocations);
                _logger.Debug("Yatse: MediaSourceID " + e.MediaSourceId);
                if (e.PlaybackPositionTicks != null)
                {
                    _logger.Debug("Yatse: PlaybackPosition Ticks " + e.PlaybackPositionTicks.HasValue);
                }

                var video = e.Item as Video;
                var progressPercent = video.RunTimeTicks.HasValue && video.RunTimeTicks != 0 ?
                    (float)(e.PlaybackPositionTicks ?? 0) / video.RunTimeTicks.Value * 100.0f : 0.0f;
                _logger.Debug("Yatse: PROGRESS PERECENT " + progressPercent);
                try
                {
                    if (video is Movie)
                    {
                        _logger.Debug("Send movie status update");
                    }
                    else if (video is Episode)
                    {
                        _logger.Debug("Send episode status update");
                    }
                }
                catch (Exception ex)
                {
                    _logger.ErrorException("Exception handled sending status update", ex);
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Error sending watching status update", ex, null);
            }
        }


        public void Dispose()
        {
            _sessionManager.PlaybackStart -= KernelPlaybackStart;
            _sessionManager.PlaybackStopped -= KernelPlaybackStart;
            _sessionManager.SessionStarted -= GetClientInfo;
        }





    }
}




