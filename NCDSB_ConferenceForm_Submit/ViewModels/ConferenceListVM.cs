﻿using NCDSB_ConferenceForm_Submit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.ViewModels
{
    public class ConferenceListVM
    {
        public ConferenceForm ConferenceForm;

        public string ConferenceName { get; set; }
    }
}