using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Trips.Domain.Entities
{
    /// <summary>
    ///  Query locations based on theme
    ///  Create planes via an algorithm that takes a time
    ///  return plans that satisfy the time quota 
    ///  give the places scores to decide how related they are to the plan
    ///  give the plans score based on the overall score of the places
    ///  let the rating contribute to the score
    ///  let the featured flag contribute to the score too
    ///  get the top X plans based on theme score
    ///  sort them based on rating and featured flag overall
    /// </summary>
    public class Place
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public double? Rating { get; set; } = null;
        public double? AverageVisitTime { get; set; } = null;
        public int VisitCount { get; set; } = 0;
        public int RateCount { get; set; } = 0;
    }
    public class PlaceTag
    {
        public int TagId { get; set; }
        public int PlaceId { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class ThemeCategory
    {
        public int Id { get; set; }
        public int ThemeId { get; set; }
        public int CategoryId { get; set; }
        public int MaxLimit { get; set; }
    }
    public class ThemeTag
    {
        public int Id { get; set; }
        public int ThemeId { get; set; }
        public int TagId { get; set; }
        public int Score { get; set; }
    }
    public class Theme
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}