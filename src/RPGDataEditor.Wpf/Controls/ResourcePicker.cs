using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Extensions.Prism.Wpf;
using RPGDataEditor.Wpf.Converters;
using RPGDataEditor.Wpf.Mvvm;
using RPGDataEditor.Wpf.Views;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class ResourcePicker : UserControl
    {
        public static DependencyProperty ResourceProperty = DependencyProperty.Register(nameof(ResourceType), typeof(Type), typeof(ResourcePicker), new PropertyMetadata(null, OnResourceChanged));
        private static void OnResourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ResourcePicker).OnResourceChanged((Type)e.OldValue, (Type)e.NewValue);
        public Type ResourceType {
            get => (Type)GetValue(ResourceProperty);
            set => SetValue(ResourceProperty, value);
        }

        public static DependencyProperty IdPathProperty = DependencyProperty.Register(nameof(IdPath), typeof(string), typeof(ResourcePicker), new PropertyMetadata("Id", OnIdPathChanged));
        private static void OnIdPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ResourcePicker).OnPickedIdChanged((int)e.OldValue, (int)e.NewValue);
        public string IdPath {
            get => (string)GetValue(IdPathProperty);
            set => SetValue(IdPathProperty, value);
        }

        public static DependencyProperty PickedItemStringFormatProperty = DependencyProperty.Register(nameof(PickedItemStringFormat), typeof(string), typeof(ResourcePicker), new PropertyMetadata("{0}"));
        public string PickedItemStringFormat {
            get => (string)GetValue(PickedItemStringFormatProperty);
            set => SetValue(PickedItemStringFormatProperty, value);
        }

        public static DependencyProperty PickedItemProperty = DependencyProperty.Register(nameof(PickedItem), typeof(object), typeof(ResourcePicker), new PropertyMetadata(null, OnPickedItemChanged));
        private static void OnPickedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ResourcePicker).OnPickedItemChanged(e.OldValue, e.NewValue);
        public object PickedItem {
            get => GetValue(PickedItemProperty);
            set => SetValue(PickedItemProperty, value);
        }

        public static DependencyProperty PickedIdProperty = DependencyProperty.Register(nameof(PickedId), typeof(object), typeof(ResourcePicker), new PropertyMetadata(-1, OnPickedIdChanged));
        private static void OnPickedIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ResourcePicker).OnPickedIdChanged(e.OldValue, e.NewValue);
        public object PickedId {
            get => GetValue(PickedIdProperty);
            set => SetValue(PickedIdProperty, value);
        }

        public static readonly DependencyProperty ModelNameConverterProperty =
            DependencyProperty.Register("ModelNameConverter", typeof(IValueConverter), typeof(ResourcePicker), new PropertyMetadata(StaticValueConverter.Create(x => x.ToString(), null)));
        public IValueConverter ModelNameConverter {
            get => (IValueConverter)GetValue(ModelNameConverterProperty);
            set => SetValue(ModelNameConverterProperty, value);
        }


        public static DependencyProperty EmptyTextProperty = DependencyProperty.Register(nameof(EmptyText), typeof(string), typeof(ResourcePicker), new PropertyMetadata("None", OnEmptyTextChanged));
        private static void OnEmptyTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ResourcePicker picker = (d as ResourcePicker);
            if (picker.PickedItem == null)
            {
                if (picker.resourceTextBlock != null)
                {
                    picker.resourceTextBlock.Text = e.NewValue as string;
                }
            }
        }
        public string EmptyText {
            get => (string)GetValue(EmptyTextProperty);
            set => SetValue(EmptyTextProperty, value);
        }

        public static DependencyPropertyKey IsLoadingPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsLoading), typeof(bool), typeof(ResourcePicker), new PropertyMetadata());
        public static DependencyProperty IsLoadingProperty = IsLoadingPropertyKey.DependencyProperty;
        public bool IsLoading {
            get => (bool)GetValue(IsLoadingProperty);
            protected set => SetValue(IsLoadingPropertyKey, value);
        }

        public static DependencyPropertyKey IsItemLoadedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsLoaded), typeof(bool), typeof(ResourcePicker), new PropertyMetadata(true));
        public static DependencyProperty IsItemLoadedProperty = IsItemLoadedPropertyKey.DependencyProperty;
        public bool IsItemLoaded {
            get => (bool)GetValue(IsItemLoadedProperty);
            protected set => SetValue(IsItemLoadedPropertyKey, value);
        }

        private TextBlock resourceTextBlock;
        private UIElement loadingOverlay;
        private Button pickButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            resourceTextBlock = Template.FindName("ResourceTextBlock", this) as TextBlock;
            loadingOverlay = Template.FindName("LoadingOverlay", this) as UIElement;
            pickButton = Template.FindName("PickButton", this) as Button;

            pickButton.Click += PickButton_Click;
            DataContextChanged += ResourcePicker_DataContextChanged;

            MultiBinding resourceTextBlockBinding = new MultiBinding() {
                Converter = StaticValueConverter.CreateMulti(x => {
                    if (x == null || x.Length != 4)
                    {
                        return "";
                    }
                    if (x[0] is bool loading)
                    {
                        if (loading)
                        {
                            return "Loading...";
                        }
                    }
                    if (x[1] != null)
                    {
                        string format = x[2].ToString();
                        if (string.IsNullOrEmpty(format) || format == "{0}")
                        {
                            return x[1].ToString();
                        }
                        else
                        {
                            LocationResourceDescriptor descriptor = new LocationResourceDescriptor(ResourceType, "", PickedItemStringFormat);
                            try
                            {
                                return descriptor.GetRelativeFullPath(x[1]);
                            }
                            catch (Exception ex)
                            {
                                return "Invalid path";
                            }
                        }
                    }
                    if (x[3] is bool isLoaded)
                    {
                        return isLoaded ? EmptyText : "Failed to load";
                    }
                    return EmptyText;
                }, null)
            };
            resourceTextBlockBinding.Bindings.Add(new Binding(nameof(IsLoading)) { Source = this });
            resourceTextBlockBinding.Bindings.Add(new Binding(nameof(PickedItem)) { Source = this });
            resourceTextBlockBinding.Bindings.Add(new Binding(nameof(PickedItemStringFormat)) { Source = this });
            resourceTextBlockBinding.Bindings.Add(new Binding(nameof(IsLoaded)) { Source = this });
            resourceTextBlock.SetBinding(TextBlock.TextProperty, resourceTextBlockBinding);

            loadingOverlay.SetBinding(VisibilityProperty, new Binding(nameof(IsLoading)) {
                Source = this,
                Converter = new BoolToVisibilityConverter()
            });
        }

        private void ResourcePicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => OnPickedItemChanged(null, PickedItem);

        protected virtual void OnPickedItemChanged(object oldValue, object newValue)
        {
            if (newValue != null)
            {
                if (!string.IsNullOrEmpty(IdPath))
                {
                    PropertyDescriptor property = TypeDescriptor.GetProperties(PickedItem).Find(IdPath, false);
                    if (property != null)
                    {
                        PickedId = property.GetValue(PickedItem);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"{nameof(IdPath)} is not valid path to Id of item {PickedItem}", "Binding");
                    }
                }
            }
        }

        protected virtual async void OnPickedIdChanged(object oldId, object newId)
        {
            if (!IsLoading)
            {
                await ReassignItemAsync();
            }
        }

        protected virtual async void OnResourceChanged(Type oldValue, Type newValue) => await ReassignItemAsync();

        protected virtual Task ReassignItemAsync()
        {
            object id = PickedId;
            if (ResourceType == null || id == null || IdentityEqualityComparer.Default.Equals(id, -1))
            {
                return Task.CompletedTask;
            }
            IsLoading = true;
            try
            {
                IDataSource dataSource = Application.Current.TryResolve<IDataSource>();
                // TODO: Fix ToArray() scaling performance problem
                object resource = dataSource.Query(ResourceType).ToArray().FirstOrDefault(x => {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(x).Find(IdPath, false);
                    object idValue = prop?.GetValue(x);
                    return IdentityEqualityComparer.Default.Equals(idValue, id);
                });
                IsLoading = false;
                IsItemLoaded = true;
                PickedItem = resource;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                IsItemLoaded = false;
            }
            return Task.CompletedTask;
        }

        private async void PickButton_Click(object sender, RoutedEventArgs e) => await PickItemAsync();

        protected virtual async Task PickItemAsync()
        {
            RPGDataEditor.Mvvm.Services.IDialogService service = Application.Current.TryResolve<RPGDataEditor.Mvvm.Services.IDialogService>();
            RPGDataEditor.Mvvm.Navigation.IDialogResult result =
                await service.ShowDialogAsync(DialogNames.PickerDialog, new PickerDialogParameters(ResourceType, PickedItem, PickedId, ModelNameConverter).Build()).ConfigureAwait(true);
            if (result.IsSuccess)
            {
                object pickedItem = result.Parameters.GetValue(nameof(PickerDialogParameters.PickedItem));
                PickedItem = pickedItem;
            }
        }
    }
}
