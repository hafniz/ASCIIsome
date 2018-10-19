using System;

namespace ASCIIsome
{
    /// <summary>The Range class. </summary>
    /// <typeparam name="T">Generic parameter that defines the type of the data held by the range instance. </typeparam>
    public class Range<T> where T : IComparable<T>
    {
        /// <summary>Constructor that provides and defines minimum and maximum value of a Range instance. </summary>
        /// <param name="minimum">Minimum value of the range. </param>
        /// <param name="maximum">Maximum value of the range. </param>
        /// <remarks>It is always the argument holding the smaller value that will be assigned to the Minimum property of the Range instance and the other argument being assigned to the Maximum property. </remarks>
        public Range(T minimum, T maximum)
        {
            if (maximum.CompareTo(minimum) == 1) // [HV] maximum is greater than minimum
            {
                Minimum = minimum;
                Maximum = maximum;
            }
            else // [HV] maximum is actually the argument of smaller value provided, OR two of the arguments are of the same value
            {
                Minimum = maximum;
                Maximum = minimum;
            }
        }

        /// <summary>Minimum value of the range. </summary>
        public T Minimum { get; set; }

        /// <summary>Maximum value of the range. </summary>
        public T Maximum { get; set; }

        /// <summary>Presents the Range in readable format. </summary>
        /// <returns>String representation of the Range instance. </returns>
        public override string ToString() => $"Range of type {nameof(T)}: {Minimum} ~ {Maximum}";

        /// <summary>Determines if the range is valid. </summary>
        /// <returns>True if range is valid, otherwise, false. </returns>
        public bool IsValid() => Minimum.CompareTo(Maximum) <= 0;

        /// <summary>Determines if the provided value is inside the range. </summary>
        /// <param name="value">The value to test. </param>
        /// <returns>True if the value is inside Range, otherwise, false. </returns>
        public bool ContainsValue(T value) => Minimum.CompareTo(value) <= 0 && value.CompareTo(Maximum) <= 0;

        /// <summary>Determines if this Range is inside the bounds of another range. </summary>
        /// <param name="Range">The parent range to test on. </param>
        /// <returns>True if range is inclusive, otherwise, false. </returns>
        public bool IsInsideRange(Range<T> range) => IsValid() && range.IsValid() && range.ContainsValue(Minimum) && range.ContainsValue(Maximum);

        /// <summary>Determines if another range is inside the bounds of this range. </summary>
        /// <param name="Range">The child range to test. </param>
        /// <returns>True if range is inside, otherwise, false.</returns>
        public bool ContainsRange(Range<T> range) => IsValid() && range.IsValid() && ContainsValue(range.Minimum) && ContainsValue(range.Maximum);
    }
}
