# CoreTimeWeather: FiveM Weather Modification Script

This script provides enhanced weather control for FiveM servers, allowing for more immersive weather transitions and effects, including a special combined "BLIZZARD" effect.

## Features

- Seamless weather transitions.
- Special handling for a combined "SNOW" and "BLIZZARD" effect.
- Server-controlled weather system that prevents interference from other sources.
- Easy configuration via `fxmanifest.lua`.

## Installation

1. Clone this repository into your FiveM `resources` directory.

`git clone https://github.com/ethanfs20/fivem_advancedsync.git`

2. Add the following line to your `server.cfg`:

`ensure fivem_advancedsync`

3. YAY!

## Configuration

If the weather type is set to "BLIZZARD" snow will also be enabled. This is to allow for a more immersive blizzard effect.

If the weather type is set to "SNOW" snow will be enabled, but the blizzard effect will not be enabled.

If the weather type is set to "THUNDER", after the thunderstorm has ended, the weather will transition to "CLEAR" weather and so too for "RAIN" weather. Eventually, the weather will transition to random type after.


## Contribution

Pull requests are welcome. For major changes, please open an issue first to discuss what you'd like to change.

## Support

For support, questions, or feedback, join our [Discord server](https://discord.gg/7eq89nUTG9).

## License

[MIT](https://choosealicense.com/licenses/mit/)


