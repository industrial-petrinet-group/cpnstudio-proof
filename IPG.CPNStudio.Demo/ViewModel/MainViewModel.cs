using GalaSoft.MvvmLight;
using System;
using System.Windows;

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
        }

        /// <summary>
        /// Create a node and add it to the view-model.
        /// </summary>
        public NodeViewModel CreateNode(string name, Point nodeLocation, bool centerNode)
        {
            var node = new NodeViewModel(name);
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
                    delegate (object sender, EventArgs e)
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
            this.Network.Nodes.Add(node);

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
            NodeViewModel node1 = CreateNode("Node1", new Point(100, 60), false);
            NodeViewModel node2 = CreateNode("Node2", new Point(350, 80), false);

            //
            // Create a connection between the nodes.
            //
            ConnectionViewModel connection = new ConnectionViewModel();
            connection.SourceConnector = node1.OutputConnectors[0];
            connection.DestConnector = node2.InputConnectors[0];

            //
            // Add the connection to the view-model.
            //
            this.Network.Connections.Add(connection);
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

    }
}