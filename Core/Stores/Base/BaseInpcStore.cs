using System;
using System.ComponentModel;
using AppInfrastructure.Stores.DefaultStore;
using DynamicData.Binding;

namespace Core.Stores.Base;

/// <summary>
///     Base realization of Istore<TValue> for INPC types 
/// </summary>
/// <typeparam name="TValue">Some INPC type</typeparam>
internal class BaseInpcStore<TValue> : BaseLazyStore<TValue> where TValue : INotifyPropertyChanged, new()
{
    public override TValue? CurrentValue
    {
        get => (TValue?)_currentValue.Value ;
        set
        {
            _currentValue = new Lazy<object?>(()=> value);
            
            CurrentValue.WhenAnyPropertyChanged()
                .Subscribe(_ => OnCurrentValueChanged());
            
            OnCurrentValueChanged();
        }
      
    }


    #region Constructors

    public BaseInpcStore(TValue value) : base(value){}

    public BaseInpcStore()
    {
        CurrentValue = new TValue();
    }

    #endregion
}