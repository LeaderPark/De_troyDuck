using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;

public class EventReciver : Receiver
{

	public override void OnNotify(Playable origin, INotification notification, object context)
	{
		base.OnNotify(origin, notification, context);
		EventMarker marker = notification as EventMarker;
		if (marker != null)
		{
            for (int i = 0; i < marker.events.Length; i++)
            {
                MethodInfo info = null;
				if (marker.events[i].obj.Resolve(origin.GetGraph().GetResolver()) != null)
				{
					foreach (var method in marker.events[i].obj.Resolve(origin.GetGraph().GetResolver()).GetComponent(marker.events[i].component).GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
					{
						if (method.Name == marker.events[i].function)
						{
							info = method;
						}
					}
				}
				if (info != null)
				{
					object[] temp = new object[marker.events[i].param.Length];

					for (int j = 0; j < temp.Length; j++)
					{
						switch (marker.events[i].param[j].type)
						{
							case "int":
							temp[j] = int.Parse(marker.events[i].param[j].data);
							break;
							case "float":
							temp[j] = float.Parse(marker.events[i].param[j].data);
							break;
							case "bool":
							temp[j] = bool.Parse(marker.events[i].param[j].data);
							break;
							case "string":
							temp[j] = marker.events[i].param[j].data;
							break;
							case "Vector2":
                            string[] ss2 = marker.events[i].param[j].data.Split('*');
							temp[j] = new Vector2(float.Parse(ss2[0]), float.Parse(ss2[1]));
							break;
							case "Vector3":
                            string[] ss3 = marker.events[i].param[j].data.Split('*');
							temp[j] = new Vector3(float.Parse(ss3[0]), float.Parse(ss3[1]), float.Parse(ss3[2]));
							break;
						}
					}

                	info.Invoke(marker.events[i].obj.Resolve(origin.GetGraph().GetResolver()).GetComponent(marker.events[i].component), temp);
				}
            }
		}
	}
}
