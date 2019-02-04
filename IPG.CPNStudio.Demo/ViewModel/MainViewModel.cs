using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Msagl.Core;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Layout.MDS;
using Microsoft.Msagl.Miscellaneous;
using System;
using System.Windows;
using System.Windows.Input;

namespace IPG.CPNStudio.Demo.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        NetworkViewModel network;

        ICommand _layoutCommand;

        CancelToken _cancelToken;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ///
            PopulateWithTestData();

            _cancelToken = new CancelToken();

            LayoutCommand = new RelayCommand(Layout );
        }

        private void Layout()
        {
            LayoutHelpers.CalculateLayout(network, GetMdsLayoutSettings(), _cancelToken);

            foreach (var node in network.NodesSource)
            {
                node.X = node.Center.X + 300;
                node.Y = node.Center.Y + 300;
            }
        }


        MdsLayoutSettings GetMdsLayoutSettings()
        {
            var settings = new MdsLayoutSettings
            {
                EdgeRoutingSettings = { KeepOriginalSpline = true, EdgeRoutingMode = EdgeRoutingMode.None },
                RemoveOverlaps = false
            };
            return settings;
        }
     

        public static ICurve CreateCurve(double x, double y, double w, double h)
        {
            var center = new Microsoft.Msagl.Core.Geometry.Point(x + (w / 2), y + (h / 2));
            return CurveFactory.CreateRectangle(w, h, center);
        }

        /// <summary>
        /// Create a node and add it to the view-model.
        /// </summary>
        public NodeViewModel CreateNode(string name, Point nodeLocation,double width, double height, bool centerNode)
        {
            var node = new NodeViewModel(CreateCurve(nodeLocation.X,nodeLocation.Y, width,height),name);
            node.X = nodeLocation.X;
            node.Y = nodeLocation.Y;

            node.InputConnectors.Add(new ConnectorViewModel("In1"));
            node.InputConnectors.Add(new ConnectorViewModel("In2"));
            node.OutputConnectors.Add(new ConnectorViewModel("Out1"));
            node.OutputConnectors.Add(new ConnectorViewModel("Out2"));

            if (centerNode)
            {
                // 
                // We want to center the node.
                //
                // For this to happen we need to wait until the UI has determined the 
                // size based on the node's data-template.
                //
                // So we define an anonymous method to handle the SizeChanged event for a node.
                //
                // Note: If you don't declare sizeChangedEventHandler before initializing it you will get
                //       an error when you try and unsubscribe the event from within the event handler.
                //
                EventHandler<EventArgs> sizeChangedEventHandler = null;
                sizeChangedEventHandler =
                    delegate(object sender, EventArgs e)
                    {
                        //
                        // This event handler will be called after the size of the node has been determined.
                        // So we can now use the size of the node to modify its position.
                        //
                        node.X -= node.Size.Width / 2;
                        node.Y -= node.Size.Height / 2;

                        //
                        // Don't forget to unhook the event, after the initial centering of the node
                        // we don't need to be notified again of any size changes.
                        //
                        node.SizeChanged -= sizeChangedEventHandler;
                    };

                //
                // Now we hook the SizeChanged event so the anonymous method is called later
                // when the size of the node has actually been determined.
                //
                node.SizeChanged += sizeChangedEventHandler;
            }

            //
            // Add the node to the view-model.
            //
            this.Network.AddNode(node);

            return node;
        }

        private void PopulateWithTestData()
        {
            //
            // Create a network, the root of the view-model.
            //
            this.Network = new NetworkViewModel();

            //
            // Create some nodes and add them to the view-model.
            //
            NodeViewModel node1 = CreateNode("Node1", new Point(100, 60),100,100, false);
            NodeViewModel node2 = CreateNode("Node2", new Point(300, 60), 100, 100, false);
            NodeViewModel node3 = CreateNode("Node3", new Point(500, 60), 100, 100, false);
            NodeViewModel node4 = CreateNode("Node4", new Point(100, 200), 100, 100, false);
            NodeViewModel node5 = CreateNode("Node5", new Point(300, 200), 100, 100, false);
            NodeViewModel node6 = CreateNode("Node6", new Point(500, 200), 100, 100, false);


            this.Network.AddConnection(new ConnectionViewModel(node1, node1.OutputConnectors[0], node2, node2.InputConnectors[0]));

            this.Network.AddConnection(new ConnectionViewModel(node2, node2.OutputConnectors[0], node3, node3.InputConnectors[0]));

            this.Network.AddConnection(new ConnectionViewModel(node2, node2.OutputConnectors[1], node4, node4.InputConnectors[0]));

            this.Network.AddConnection(new ConnectionViewModel(node4, node4.OutputConnectors[0], node5, node5.InputConnectors[0]));

            this.Network.AddConnection(new ConnectionViewModel(node4, node4.OutputConnectors[1], node6, node6.InputConnectors[0]));

       

        }


        /// <summary>
        /// This is the network that is displayed in the window.
        /// It is the main part of the view-model.
        /// </summary>
        public NetworkViewModel Network
        {
            get
            {
                return network;
            }
            set
            {
                network = value;

                RaisePropertyChanged("Network");
            }
        }

        public ICommand LayoutCommand
        {
            get
            {
                return _layoutCommand;
            }
            set
            {
                _layoutCommand = value;

                RaisePropertyChanged("LayoutCommand");
            }
           }
    }
}