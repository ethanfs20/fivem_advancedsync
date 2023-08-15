using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace CoreTimeWeather.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            // Register the event handler for when the players spawns.
            EventHandlers["playerSpawned"] += new Action(OnPlayerSpawned);
            // Register event to handle the time and weather from the server.
            // player.TriggerEvent("fivem_advancedsync:client:Sync", now.Hour, now.Minute, now.Second, TIME_UPDATE_INTERVAL);
            EventHandlers["fivem_advancedsync:client:Sync"] += new Action<int, int, int, int, int, int, int, string>(SyncTimeWeather);
            // TriggerClientEvent("fivem_advancedsync:client:WeatherRandom", weather);
            EventHandlers["fivem_advancedsync:client:Weather"] += new Action<string>(SetWeather);
        }

        // This function is called when the player spawns to trigger the event to get the time and weather.
        // simple command to test
        private void OnPlayerSpawned()
        {
            // Simple debugggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggghelpme
            // Debug.WriteLine("Player spawned");
            TriggerServerEvent("fivem_advancedsync:server:Sync");
        }

        // This function is called when the server sends the time and weather to the client.
        private void SyncTimeWeather(int hour, int minute, int second, int month, int day, int year, int interval, string weather)
        {
            // This will set the time according to what is provided by the server.
            AdvanceClockTimeTo(hour, minute, second);
            // Small wait to make because the above does a smooth transition.
            NetworkOverrideClockTime(hour, minute, second);
            // This will set the date according to what is provided by the server.
            SetClockDate(day, month, year);
            // This will set the time scale according to what is provided by the server.
            NetworkOverrideClockMillisecondsPerGameMinute(interval);
            // This will set the weather according to what is provided by the server. 1.0f is the transition time in seconds.
            SetWeather(weather);
            // MOAR DAYBUGANDEENG!>?
            Debug.WriteLine($"Time: {hour}:{minute}:{second} Date: {day}/{month}/{year} Interval: {interval}");
        }

        // This function is called when the server sends the weather to the client.
        private void SetWeather(string weather)
        {
            Debug.WriteLine($"Weather: {weather}");
            SetWeatherOwnedByNetwork(false);
            ClearWeatherTypePersist();

            if (weather == "BLIZZARD")
            {
                SetWeatherTypeOvertimePersist(weather, 10.0f);
                ForceSnowPass(true);
            }
            else
            {
                SetWeatherTypeOvertimePersist(weather, 10.0f);
            }
        }
    }
}
