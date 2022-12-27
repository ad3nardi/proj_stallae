using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class bgenerator
{
    public static building Generate(bgen_bldgSettings settings)
    {
        return new building(settings.size.x, settings.size.y, GenerateWings(settings));
    }
    static wing[] GenerateWings(bgen_bldgSettings settings)
    {
        return new wing[] { GenerateWing(settings, new RectInt(0,0, settings.size.x, settings.size.y), 1) };
    }

    static wing GenerateWing(bgen_bldgSettings settings, RectInt bounds, int numberOfStories)
    {
        return new wing
            (
                bounds,
                GenerateStories(settings, bounds, numberOfStories),
                settings.roofStrategy != null ?
                    settings.roofStrategy.GenerateRoof(settings, bounds) :
                    ((bgen_roofStrat)ScriptableObject.CreateInstance<bgen_defaultRoof>()).GenerateRoof(settings, bounds));
            
    }

    static story[] GenerateStories(bgen_bldgSettings settings, RectInt bounds, int numberOfStories)
    {
        return new story[] { GenerateStory(settings, bounds, 1) };
    }

    static story GenerateStory(bgen_bldgSettings settings, RectInt bounds, int level)
    {
        return new story(0, GenerateWalls(settings, bounds, level));
    }


    static wall[] GenerateWalls(bgen_bldgSettings settings, RectInt bounds, int level)
    {
        return new wall[(bounds.size.x + bounds.size.y) * 2];
    }

    //static roof GenerateRoof(bgen_bldgSettings settings, RectInt bounds)
    //{
    //    return new roof();
    //}

}