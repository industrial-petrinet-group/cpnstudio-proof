using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IPG.CPNStudio.Diagram
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:IPG.CPNStudio.Diagram"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:IPG.CPNStudio.Diagram;assembly=IPG.CPNStudio.Diagram"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DiagramControl/>
    ///
    /// </summary>
    public class DiagramControl : Control
    {
        private static readonly DependencyPropertyKey NodesPropertyKey =
          DependencyProperty.RegisterReadOnly("Nodes", typeof(ObservableCollection<object>), typeof(DiagramControl),
              new FrameworkPropertyMetadata());
        public static readonly DependencyProperty NodesProperty = NodesPropertyKey.DependencyProperty;


        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DiagramControl),
                new FrameworkPropertyMetadata(ItemsSource_PropertyChanged));

        private static void ItemsSource_PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            DiagramControl diagramControl = (DiagramControl)dependencyObject;

            //
            // Clear 'Nodes'.
            //
            diagramControl.Nodes.Clear();

            if (e.OldValue != null)
            {
                var notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Unhook events from previous collection.
                    //
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(diagramControl.NodesSource_CollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                var enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    //
                    // Populate 'Nodes' from 'ItemsSource'.
                    //
                    foreach (object obj in enumerable)
                    {
                        diagramControl.Nodes.Add(obj);
                    }
                }

                var notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Hook events in new collection.
                    //
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(diagramControl.NodesSource_CollectionChanged);
                }
            }
        }


        /// <summary>
        /// Event raised when a node has been added to or removed from the collection assigned to 'NodesSource'.
        /// </summary>
        private void NodesSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //
                // 'NodesSource' has been cleared, also clear 'Nodes'.
                //
                Nodes.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {
                    //
                    // For each item that has been removed from 'NodesSource' also remove it from 'Nodes'.
                    //
                    foreach (object obj in e.OldItems)
                    {
                        Nodes.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {
                    //
                    // For each item that has been added to 'NodesSource' also add it to 'Nodes'.
                    //
                    foreach (object obj in e.NewItems)
                    {
                        Nodes.Add(obj);
                    }
                }
            }
        }

        public  ObservableCollection<object> Nodes
        {
            get
            {
                return (ObservableCollection<object>)GetValue(NodesProperty);
            }
            private set
            {
                SetValue(NodesPropertyKey, value);
            }
        }

        static DiagramControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DiagramControl), new FrameworkPropertyMetadata(typeof(DiagramControl)));
        }
    }
}
