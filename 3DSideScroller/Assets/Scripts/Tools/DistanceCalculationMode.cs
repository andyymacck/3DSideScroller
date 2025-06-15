namespace SideScroller
{
    /// <summary>
    /// Distance calculation modes for path calculations
    /// </summary>
    public enum DistanceCalculationMode
    {
        /// <summary>Use all three axes (X, Y, Z) for distance calculation</summary>
        ThreeDimensional,
        
        /// <summary>Ignore X axis, use only Y and Z for distance calculation</summary>
        IgnoreX,
        
        /// <summary>Ignore Y axis, use only X and Z for distance calculation</summary>
        IgnoreY,
        
        /// <summary>Ignore Z axis, use only X and Y for distance calculation</summary>
        IgnoreZ
    }
}