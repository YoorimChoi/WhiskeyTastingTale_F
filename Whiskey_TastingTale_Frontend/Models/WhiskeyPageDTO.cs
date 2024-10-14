﻿using Whiskey_TastingTale_Backend.Model;

namespace Whiskey_TastingTale_Backend.API.DTOs
{
    public class WhiskeyPageDTO
    {
        public List<Whiskey> whiskeys { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public int totalPages { get; set; } 
    }
}
