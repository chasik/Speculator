using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace Speculator.DataModel
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    public abstract class SampleDataCommon : ViewModelBase
    {
        private static Uri _baseUri = new Uri("pack://application:,,,");
        static BitmapImage GetImage(string path)
        {
#if SILVERLIGHT
            return new BitmapImage(new Uri("../"  + path, UriKind.RelativeOrAbsolute));
#else
            return new BitmapImage(new Uri(_baseUri, path));
#endif
        }
        static int count;
        static string GetUniqueId()
        {
            return "Item" + count++;
        }
        public SampleDataCommon(String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = GetUniqueId();
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }
        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
        }
        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value, "Title"); }
        }
        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value, "Subtitle"); }
        }
        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value, "Description"); }
        }
        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = SampleDataCommon.GetImage(_imagePath);
                }
                return this._image;
            }
            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value, "Image");
            }
        }
        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String title, String subtitle, String imagePath, String description, String content)
            : this(title, subtitle, imagePath, description, content, false, string.Empty)
        {

        }
        public SampleDataItem(String title, String subtitle, String imagePath, String description, String content, bool isFlowBreak, string groupHeader)
            : base(title, subtitle, imagePath, description)
        {
            this._content = content;
            this.IsFlowBreak = isFlowBreak;
            this.GroupHeader = groupHeader;
        }
        private string _GroupHeader;
        private bool _IsFlowBreak;
        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value, "Content"); }
        }

        public bool IsFlowBreak
        {
            get { return _IsFlowBreak; }
            set { this.SetProperty(ref this._IsFlowBreak, value, "IsFlowBreak"); }
        }

        public string GroupHeader
        {
            get { return _GroupHeader; }
            set { this.SetProperty(ref this._GroupHeader, value, "GroupHeader"); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    ///
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class SampleDataSource
    {
        public static SampleDataSource Instance
        {
            get { return _sampleDataSource; }
        }
        static readonly SampleDataSource _sampleDataSource = new SampleDataSource();

        ObservableCollection<SampleDataItem> _items;
        public ObservableCollection<SampleDataItem> Items
        {
            get { return Instance._items; }
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = SampleDataSource.Instance.Items.Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
            _items = new ObservableCollection<SampleDataItem>();
            string ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");
            string ITEM_DESCRIPTION = "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.";

            _items.Add(new SampleDataItem(
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/LightGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT, true, "Group 1"));
            _items.Add(new SampleDataItem(
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/MediumGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 5",
                    "Item Subtitle: 5",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 6",
                    "Item Subtitle: 6",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));


            _items.Add(new SampleDataItem(
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/MediumGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT, true, "Group 2"));
            _items.Add(new SampleDataItem(
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));

            _items.Add(new SampleDataItem(
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/MediumGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT, true, "Group 3"));
            _items.Add(new SampleDataItem(
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/LightGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 5",
                    "Item Subtitle: 5",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 6",
                    "Item Subtitle: 6",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 7",
                    "Item Subtitle: 7",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 8",
                    "Item Subtitle: 8",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));

            _items.Add(new SampleDataItem(
                   "Item Title: 1",
                   "Item Subtitle: 1",
                   "Assets/MediumGray.png",
                   ITEM_DESCRIPTION,
                   ITEM_CONTENT, true, "Group 4"));
            _items.Add(new SampleDataItem(
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/LightGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
            _items.Add(new SampleDataItem(
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/DarkGray.png",
                    ITEM_DESCRIPTION,
                    ITEM_CONTENT));
        }
    }
}
