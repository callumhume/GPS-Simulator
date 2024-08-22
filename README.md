# GPS-Simulator

## Introduction

This project is a shameless recreation / improvement upon the GPS simulator created forever and a day ago by Ag Leader.

### Feature set

- Speed ramp to target,
- Dynamic turn radius based on speed (degrees per second),
- Demonstration driving modes,
- Configurable projection,
- Configurable number of satellites in use,
- Configurable fix type (quality indicator),
- Tracked total distance,

### Roadmap

- Selectable demo drive mode (currently hard-coded selection from five available modes),
- Selectable unit system (currently metric-only),
- Dynamic WGS-84 compensation (currently only works near 45 degrees latitude),
- Configurable swath width,
    - Automatic back-and-forth driving,
        - Selectable turn-around method (large circle or three-point),
    - Swath painting on map,
    - Track covered area,
- Configurable starting coordinates,
- Field boundaries,
- Hotkeys for manual driving,
- Quick stop/go/turn buttons, separate from cardinal direction buttons,
- VTG sentence output

### License

Currently, this project uses the [GPLv3](https://www.gnu.org/licenses/gpl-3.0.en.html) license.  At some point in the future, likely before any full production releases, it may be switched to one of the [Fair Source licenses](https://fair.io/licenses/).