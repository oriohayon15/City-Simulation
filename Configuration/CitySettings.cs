namespace CitySimulation.Configuration;

public static class CitySettings
{
    public const int InitialYear = 2026;

    public const int GridWidth = 12;
    public const int GridHeight = 12;

    public const int InitialResidentialBlocks = 25;
    public const int InitialWorkplaceBlocks = 20;
    public const int InitialWaterBlocks = 25;
    public const int InitialSchoolBlocks = 5;
    public const int InitialEmptyBlocks = 69;
    public const int InitialCityLayoutSeed = 1;

    public const int InitialPopulation = 10_000;
    public const int ResidentialCapacity = 425;
    public const int WorkplaceCapacity = 400;
    public const int SchoolCapacity = 500;
    public const int TeachersPerSchool = 15;

    public const double InitialAge0To17Percentage = 0.20;
    public const double InitialAge18To40Percentage = 0.35;
    public const double InitialAge41To65Percentage = 0.30;
    public const double InitialAge66PlusPercentage = 0.15;

    // The plan defines the oldest group as 66+ but does not specify an upper bound.
    public const int InitialMaximumAge = 100;
}
