/*

   Copyright 2019 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.GlobeCore;

namespace RSSWeatherLayer3D
{
	/// <summary>
	/// Add a new weather item given a zipCode.
	/// </summary>
  /// <remarks>Should the weather item exist, it will be updated</remarks>
	[ClassInterface(ClassInterfaceType.None)]
  [Guid("3C9766F8-B6FC-45ec-AD79-A94771C1604F")]
	[ProgId("RSSWeatherLayer3D.AddWeatherItemCmd")]
  [ComVisible(true)]
	public sealed class AddWeatherItemCmd: BaseCommand
	{
		#region COM Registration Function(s)
		[ComRegisterFunction()]
		[ComVisible(false)]
		static void RegisterFunction(Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryRegistration(registerType);

			//
			// TODO: Add any COM registration code here
			//
		}

		[ComUnregisterFunction()]
		[ComVisible(false)]
		static void UnregisterFunction(Type registerType)
		{
			// Required for ArcGIS Component Category Registrar support
			ArcGISCategoryUnregistration(registerType);

			//
			// TODO: Add any COM unregistration code here
			//
		}

		#region ArcGIS Component Category Registrar generated code
		/// <summary>
		/// Required method for ArcGIS Component Category registration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryRegistration(Type registerType)
		{
			string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
      GMxCommands.Register(regKey);
      ControlsCommands.Register(regKey);
		}
		/// <summary>
		/// Required method for ArcGIS Component Category unregistration -
		/// Do not modify the contents of this method with the code editor.
		/// </summary>
		private static void ArcGISCategoryUnregistration(Type registerType)
		{
			string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
      GMxCommands.Unregister(regKey);
      ControlsCommands.Unregister(regKey);
		}

		#endregion
		#endregion

    //class members
    private IGlobeHookHelper          m_globeHookHelper = null;
		private RSSWeatherLayer3DClass	m_weatherLayer		= null;
    private IScene                    m_scene           = null;

    /// <summary>
    /// CTor
    /// </summary>
		public AddWeatherItemCmd()
		{
			base.m_category = "Weather3D";
			base.m_caption = "Add by zipcode";
			base.m_message = "Add weather item by zipcode";
			base.m_toolTip = "Add by zipcode";
			base.m_name = base.m_category + "_" + base.m_caption;

			try
			{
        base.m_bitmap = new Bitmap(GetType().Assembly.GetManifestResourceStream(GetType(), "Bitmaps.AddWeatherItemCmd.bmp"));
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
			}
		}

		#region Overriden Class Methods

		/// <summary>
		/// Occurs when this command is created
		/// </summary>
		/// <param name="hook">Instance of the application</param>
		public override void OnCreate(object hook)
		{
      //Instantiate the hook helper
      if (null == m_globeHookHelper)
        m_globeHookHelper = new GlobeHookHelperClass();

      //set the hook
      m_globeHookHelper.Hook = hook;

      m_scene = m_globeHookHelper.Globe as IScene;
		}

		/// <summary>
		/// Occurs when this command is clicked
		/// </summary>
		public override void OnClick()
		{
			try
			{				
				//get the weather layer
				IEnumLayer layers = m_scene.get_Layers(null, false);
				layers.Reset();
				ILayer layer = layers.Next();
				while(layer != null)
				{
					if(layer is RSSWeatherLayer3DClass)
					{
						m_weatherLayer = (RSSWeatherLayer3DClass)layer;
						break;
					}
					layer = layers.Next();
				}

        //in case that the layer exists
				if(null != m_weatherLayer)
				{
          //launch the zipCode input dialog
					ZipCodeDlg dlg = new ZipCodeDlg();
					if(DialogResult.OK == dlg.ShowDialog())
					{
						long zipCode = dlg.ZipCode;
						if(0 != zipCode)
						{
							//add the weather item to the layer
							m_weatherLayer.AddItem(zipCode);		

              //if the user checked the 'ZoomTo' checkbox, zoom to the item
							if(dlg.ZoomToItem)
							{
								m_weatherLayer.ZoomTo(zipCode);
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(ex.Message);
			}
		}

		#endregion
	}
}
