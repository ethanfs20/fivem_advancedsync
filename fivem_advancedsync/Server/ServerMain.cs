using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace FiveMAdvancedSync.Server
{
    public class ServerMain : BaseScript
    {
        private static readonly string[] WeatherTypes =
        {
            "EXTRASUNNY", "CLEAR", "CLOUDS", "SMOG", "FOGGY", "OVERCAST",
            "RAIN", "THUNDER", "NEUTRAL" // Dont put CLEARING, SNOW, BLIZZARD, SNOWLIGHT, XMAS, HALLOWEEN, or XMAS2 here as they are not supported in terms of functionality in this resource.
        };

        // Convar member variables
        private readonly int _timeUpdateInterval;
        private readonly string _weatherDefault;
        private readonly string _timeZone;
        private readonly int _weatherUpdateInterval;
        private readonly Random _random = new Random();

        // Current and Last Weather member variables
        private string _currentWeather;
        private string _lastWeather;

        public ServerMain()
        {
            // Get convars

            // This is the interval in which the time is updated on the client in milliseconds. Default is 60 seconds/1 minute.
            _timeUpdateInterval = GetConvarInt("TIME_UPDATE_INTERVAL", 60000);

            // This is the default weather that is set when the resource starts. Default is EXTRASUNNY if one is not provided.
            _weatherDefault = GetConvar("WEATHER_DEFAULT", "EXTRASUNNY");

            // This is the timezone that is used to calculate the time. Default is local if one is not provided. Local is the timezone of the server.
            _timeZone = GetConvar("TIMEZONE", "local");

            // This is the interval in which the weather is updated on the client in milliseconds. Default is 2 hours.
            _weatherUpdateInterval = GetConvarInt("WEATHER_UPDATE_INTERVAL", 7200000);

            // Set the current weather to the default weather.
            _currentWeather = _weatherDefault;

            // Register the event handler for the client event.
            EventHandlers["fivem_advancedsync:server:Sync"] += new Action<Player>(OnRealTimeEvent);

            // Start the weather thread.
            if (_weatherUpdateInterval > 0)
            {
                _ = WeatherThread();
            }

            RegisterCommand("changeweather", new Action<int, List<object>, string>((src, args, raw) =>
            {
                if (args.Count > 0)
                {
                    string weatherType = args[0].ToString().ToUpper();
                    if (WeatherTypes.Contains(weatherType))
                    {
                        ChangeWeather(weatherType);
                        Debug.WriteLine($"Weather changed to {weatherType} by player {src}.");
                    }
                    else
                    {
                        Debug.WriteLine($"Invalid weather type provided by player {src}.");
                    }
                }
                else
                {
                    Debug.WriteLine($"No weather type provided by player {src}.");
                }
            }), false);
        }

        // This is the event handler for the client event.
        private void OnRealTimeEvent([FromSource] Player player)
        {
            DateTime now = GetCurrentTimeBasedOnTimeZone();
            player.TriggerEvent("fivem_advancedsync:client:Sync",
                                now.Hour, now.Minute, now.Second,
                                now.Day, now.Month, now.Year,
                                _timeUpdateInterval, _currentWeather);
        }

        // This is the method that gets the current time based on the timezone.
        private DateTime GetCurrentTimeBasedOnTimeZone()
        {
            return _timeZone == "local"
                ? DateTime.Now
                : TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, _timeZone);
        }

        // This is the thread that updates the weather every _weatherUpdateInterval milliseconds.
        private async Task WeatherThread()
        {
            while (true)
            {
                ChangeWeather("RANDOM");
                await Delay(_weatherUpdateInterval);

            }
        }
        // Method to change the weather based on if called by the command or thread to change randomly. The thread will pass a string "RANDOM" to the method to change randomly. The command will pass a string of the weather type.
        private void ChangeWeather(string weather)
        {

            // Update _lastWeather with _currentWeather value
            _lastWeather = _currentWeather;

            // If the last weather was RAIN or THUNDER, then set the weather to CLEARING for an immersive EXP. ARRRPEEEE
            if (_lastWeather == "RAIN" || _lastWeather == "THUNDER")
            {
                _currentWeather = "CLEARING";
            }
            else
            {
                // If the weather is RANDOM, then set the weather to a random weather type.
                if (weather == "RANDOM")
                {
                    _currentWeather = WeatherTypes[_random.Next(WeatherTypes.Length)];
                }
                // Otherwise set the weather to the weather type passed in.
                else
                {
                    _currentWeather = weather;
                }
            }
            TriggerClientEvent("fivem_advancedsync:client:Weather", _currentWeather);
        }
    }
}
