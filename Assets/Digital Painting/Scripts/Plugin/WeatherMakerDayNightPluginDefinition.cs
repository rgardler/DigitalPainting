﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.plugin
{
    public class WeatherMakerDayNightPluginDefinition : AbstractDayNightPluginDefinition
    {
        public override string GetPluginImplementationClassName()
        {
            return "WeatherMakerScript";
        }

        public override string GetReadableName()
        {
            return "Day Night from Weather Maker";
        }

        public override string GetURL()
        {
            return "https://assetstore.unity.com/packages/tools/particles-effects/weather-maker-unity-weather-system-sky-water-volumetric-clouds-a-60955";
        }
    }
}