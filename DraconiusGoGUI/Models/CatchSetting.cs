﻿using DracoProtos.Core.Base;
using System;

namespace DraconiusGoGUI.Models
{
    [Serializable]
    public class CatchSetting
    {
        public CreatureType Id { get; set; }
        public bool Catch { get; set; }
        public bool UsePinap { get; set; }
        public bool Snipe { get; set; }

        public CatchSetting()
        {
            Id = 0;
            Catch = true;
            //Snipe = true;
            UsePinap = true;
        }

        public string Name
        {
            get
            {
                return Id.ToString();
            }
        }
    }
}
