using System.Diagnostics.Tracing;

/// <summary>
///     Provides change events for a value of Type T.
/// </summary>
/// <typeparam name="T">The value type of this value.</typeparam>
public interface IValue<T>
{
    public delegate void ValueEvent(EventSource source, T oldValue, T newValue);

    /// <summary>
    /// Get or set the value. ChangeEvents are fired without a source set.
    /// If the event source needs to be known, use SetValueWithSource()
    /// </summary>
    public abstract T Value { get; set; }

    public event ValueEvent ValueChanged;

    /// <summary>
    ///     Set the value firing an event, with explicit event source 
    /// </summary>
    /// <param name="source">The event source to use</param>
    /// <param name="newValue">new value</param>
    public bool SetValueWithSource(EventSource source, T newValue);

    /// <summary>
    ///     Set the value without firing and event.
    /// </summary>
    /// <param name="newVal">The new value.</param>
    public void SetValueWithoutNotify(T newVal);
}
