using UnityEngine;
using UnityEngine.InputSystem;


public class GlobalSelection : MonoBehaviour
{
    [HideInInspector] public bool StackActionHold = false;

    private SelectedDico selected_table;
    private RaycastHit hit;

    private bool dragSelect;

    //Collider variables
    //=======================================================//

    private MeshCollider selectionBox;
    private Mesh selectionMesh;

    private Vector3 p1;
    private Vector3 p2;

    //the corners of our 2d selection box
    private Vector2[] corners;

    //the vertices of our meshcollider
    private Vector3[] verts;
    private Vector3[] vecs;

    private int nbAddedBoxing = 0;


    // Start is called before the first frame update
    void Start()
    {
        selected_table = GetComponent<SelectedDico>();
        dragSelect = false;
    }


    // Update is called once per frame
    void Update()
    {
        //1. when left mouse button clicked (but not released)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            p1 = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
        }

        //2. while left mouse button held
        if (Mouse.current.leftButton.isPressed)
        {
            if((p1 - new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0)).magnitude > 20 && InputReceiver.instance.lastKeyPressed == '\0')
            {
                dragSelect = true;
            }
        }

        //3. when mouse button comes up
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if(dragSelect == false) //single select
            {
                if (InputReceiver.instance.lastKeyPressed != '\0')
                {
                    InputReceiver.instance.ChooseAction();
                    return;
                }

                Ray ray = Camera.main.ScreenPointToRay(p1);

                int layerMask = LayerMask.GetMask("agent", "building");
                if (Physics.Raycast(ray,out hit, 1000f, layerMask))
                {
                    Selectable hitSelectable = hit.transform.GetComponent<Selectable>();
                    if (hitSelectable != null && hitSelectable.isAlly)
                    {
                        if (hit.transform.GetComponent<AgentManager>())
                        {
                            if (StackActionHold) //inclusive select
                            {
                                selected_table.AddSelected(hit.transform.GetComponent<AgentManager>());
                            }
                            else //exclusive selected
                            {
                                selected_table.DeselectAll();
                                selected_table.AddSelected(hit.transform.GetComponent<AgentManager>());
                            }
                        }
                        else if (hit.transform.GetComponent<BuildingManager>())
                        {
                            if (StackActionHold) //inclusive select
                            {
                                selected_table.AddSelected(hit.transform.GetComponent<BuildingManager>());
                            }
                            else //exclusive selected
                            {
                                selected_table.DeselectAll();
                                selected_table.AddSelected(hit.transform.GetComponent<BuildingManager>());
                            }
                        }
                    }
                }
                else //if we didnt hit something
                {
                    //do nothing
                }
            }
            else //box select
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                p2 = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
                corners = getBoundingBox(p1, p2);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out hit, 1000f, (1 << 8)))
                    {
                        verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        vecs[i] = ray.origin - hit.point;
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                    }
                    i++;
                }

                //generate the mesh
                selectionMesh = generateSelectionMesh(verts,vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if (!StackActionHold && nbAddedBoxing != 0)
                {
                    selected_table.DeselectAll();
                }

               Destroy(selectionBox, 0.02f);

            }//end marquee select

            dragSelect = false;
            nbAddedBoxing = 0;
        }
       
    }


    private void OnGUI()
    {
        if(dragSelect == true)
        {
            var rect = RectDrawer.GetScreenRect(p1, new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0));
            RectDrawer.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            RectDrawer.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }


    //create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] getBoundingBox(Vector2 p1,Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }


    //generate a mesh from the 4 bottom points
    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for(int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for(int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }


    private void OnTriggerEnter(Collider other)
    {
        AgentManager hitAgent = other.transform.GetComponent<AgentManager>();
        BuildingManager hitBuilding = other.transform.GetComponent<BuildingManager>();

        if ((hitAgent != null && hitAgent.isAlly) || (hitBuilding != null && hitBuilding.isAlly))
        {
            selected_table.AddSelected(other.GetComponent<AgentManager>());
            nbAddedBoxing++;
        }
    }
}
