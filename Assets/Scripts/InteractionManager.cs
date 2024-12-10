using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using TouchControl = UnityEngine.InputSystem.Controls.TouchControl;
using TouchPhaseControl = UnityEngine.InputSystem.Controls.TouchPhaseControl;
using TopDownCameraController;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEditor;
using System;
using Area;
using TMPro;
using UnityEngine.EventSystems;

namespace Mapbox.Examples
{
    public class InteractionManager : MonoBehaviour
    {

        #region public variables
        //Text
        public TextMeshProUGUI m_west;
        public TextMeshProUGUI m_ost;
        public TextMeshProUGUI m_north;
        public TMP_InputField m_south;

        //Generals
        public AbstractMap map;
        public GameObject m_fire_truck;
        public GameObject m_filling_brush;
        public GameObject m_line_brush;
        public Button save_pen_drawing_button;

        public RenderTexture save_filled_texture;
        public Material _area_material;

        //Drag Taktische Zeichen
        public Sprite[] taktische_zeichnen;
        public GameObject m_dragTG;

        //icon tactical graphics
        public bool m_placingIcons;

        //area marking
        public bool m_erase;
        public bool m_drawLine;
        public PenDrawingBehavior m_customPen;

        //incident area UGUI
        public Button area_menu;
        public GameObject m_back_button_Area;
        public GameObject m_incident_area_content;
        public GameObject m_incident_area_content_content;
        public GameObject m_incident_button;

        //custom tactical graphics
        public GameObject m_custom_TG;
        public GameObject m_custom_TG_prefab;

        //Cameras
        public Camera m_camera_3D;
        public Camera m_camera_2D;

        //Marking Building
        public bool m_markingBuilding;


        public float m_camera_2D_height;
        public Transform m_transform;
        public GameObject touch_point;
        public bool m_hit_sticker;
        public GameObject hit = null;




        //Area Shading
        public GameObject m_paintBrush;
        public GameObject m_paintBrushHolder;
        public GameObject m_shadingContainer_temp = null;
        public Material m_materialArea_pref;
        public Camera m_AreaCamera;

        //Tacal Graphic folders
        public GameObject m_tactical_folder;
        #endregion
        #region private variables
        private LineRenderer m_line_renderer;
        private GameObject filled_area = null;
        private GameObject pref = null;
        private Vector2d real_LatLong;
        private float touch_time;
        Vector3 prev_pos = Vector3.zero;
        private Plane rotationPlane;
        [SerializeField]
        private Vector3 offset;
        private Plane _yPlane;
        private bool m_rotate_state;
        [SerializeField]
        private Vector3 refVelocity;
        private float m_smooth_speed = 4f;
        private bool m_tile_not_updated;
        [SerializeField]
        private List<Vector3> brush_points;
        private GameObject curr_brush_tile;
        private GameObject button;

        //Area Marking & Shading
        public List<GameObject> m_incident_areas_List;
        public List<GameObject> m_incident_line_List;
        private RenderTexture m_renderTexture;


        private EventSystem es;
        #endregion
        #region getters/setters
        public Vector3 Offset { get => offset; set => offset = value; }
        public Vector3 PlaneDeltaPosition(TouchControl touch)
        {
            return OnPlanePositionDelta(touch);
        }
        public Vector3 PlanePosFromScreenPoint(Vector2 screen_point)
        {
            return OnPlanePositionFromScreenPoint(screen_point);
        }
        #endregion

        #region main methods
        private void Awake()
        {
            EnhancedTouchSupport.Enable();
            es = FindObjectOfType<EventSystem>();
        }
        public void Start()
        {
            _yPlane.SetNormalAndPosition(transform.up, transform.position);
            Offset = Vector3.zero;
            rotationPlane.SetNormalAndPosition(transform.up, transform.position);
            m_camera_2D_height = (m_camera_2D.gameObject.GetComponent<TopDwonCameraController>()).m_height;
            save_pen_drawing_button.gameObject.SetActive(false);
            m_hit_sticker = false;
            touch_point.transform.position = transform.position;

            m_incident_areas_List = new List<GameObject>();
            m_incident_line_List = new List<GameObject>();

            m_incident_area_content.SetActive(false);
            m_back_button_Area.SetActive(false);
            area_menu.gameObject.SetActive(false);
            m_custom_TG = null;
            m_markingBuilding = false;
            m_erase = false;
            m_placingIcons = false;
            m_drawLine = false;

            m_west.gameObject.SetActive(false);
            m_ost.gameObject.SetActive(false);
            m_north.gameObject.SetActive(false);
            m_south.gameObject.SetActive(false);
        }
        public void Update()
        {
            transform.position.Set(transform.position.x, 0f, transform.position.z);
            var mouseInput = Mouse.current;
            var touchInput = Touchscreen.current;
            var penInput = Pen.current;
            if (touchInput == null)
            {
                return;
            }
            var deltaOne = Vector3.zero;
            m_transform = transform;

            if (m_incident_areas_List.Count > 0)
            {
                area_menu.gameObject.SetActive(true);
            }

            //Finger Touch
            if (touchInput.wasUpdatedThisFrame && !m_incident_area_content.activeSelf && !m_markingBuilding && !m_placingIcons && !m_drawLine && !m_erase && !es.alreadySelecting && es.currentSelectedGameObject == null)
            {
                if (m_camera_2D.gameObject.activeSelf)
                {
                    transform.forward = map.transform.forward;
                    //One finger --> scroll to move
                    if (touchInput.touches.ToArray()[0].isInProgress && !touchInput.touches.ToArray()[1].isInProgress && !touchInput.touches.ToArray()[2].isInProgress)
                    {
                        Vector2 screen_pos = touchInput.touches.ToArray()[0].position.ReadValue();
                        OnMoveCamera2D(touchInput, deltaOne, m_transform);
                    }
                    //Two fingers --> pinch to zoom
                    else if (touchInput.touches.ToArray()[0].isInProgress && touchInput.touches.ToArray()[1].isInProgress && !touchInput.touches.ToArray()[2].isInProgress && m_custom_TG == null)
                    {
                        OnZoomCamera2D(touchInput);
                    }
                    //Three fingers --> Custom tactical graphics
                    else if (touchInput.touches.ToArray()[0].isInProgress && touchInput.touches.ToArray()[1].isInProgress && touchInput.touches.ToArray()[2].isInProgress && m_custom_TG == null)
                    {
                        OnCustomTacticalGraphic(touchInput, deltaOne);
                    }
                }

                else if (m_camera_3D.gameObject.activeSelf && !m_drawLine)
                {
                    //One finger pressed --> move
                    if (touchInput.touches.ToArray()[0].isInProgress && !touchInput.touches.ToArray()[1].isInProgress)
                    {
                        OnMoveCamera3D(touchInput, deltaOne, m_transform);
                    }

                    //Two fingers pressed --> rotate
                    else if (touchInput.touches.ToArray()[0].isInProgress && touchInput.touches.ToArray()[1].isInProgress)
                    {
                        OnRotateCamera3D(touchInput);
                    }
                }
            }
            else if (m_drawLine && !m_markingBuilding && !m_incident_area_content.activeSelf && !m_erase && !m_placingIcons)
            {
                if (penInput != null && penInput.wasUpdatedThisFrame)
                {
                    switch (m_customPen.m_drawMode)
                    {
                        case 0:
                            OnPenInputDraw(penInput);
                            break;
                        case 1:
                            OnPenInputFillArea(penInput);
                            break;
                    }

                }
                else if (touchInput.wasUpdatedThisFrame && m_customPen._canDrawNow >= 3 && !m_customPen.m_colorSelected.sprite.name.Contains("notSelected")
                     && !m_customPen.m_sizeSelected.sprite.name.Contains("notSelected") && !m_customPen.m_modeSelected.sprite.name.Contains("notSelected")
                     && !es.alreadySelecting && es.currentSelectedGameObject == null)
                {
                    switch (m_customPen.m_drawMode)
                    {
                        case 0:
                            OnFingerInputDraw(touchInput);
                            break;
                        case 1:
                            OnFingerInputFillArea(touchInput);
                            break;
                    }
                }
            }


            //Mouse Press
            //else if (mouseInput.wasUpdatedThisFrame)
            //{
            //	//Custom TG creation
            //	if (mouseInput.rightButton.isPressed)
            //	{
            //		touch_point.transform.position = OnPlanePositionFromScreenPoint(mouseInput.position.ReadValue());
            //		OnMouseCTG(mouseInput);
            //	}
            //	//Move 
            //	else if (mouseInput.leftButton.isPressed && m_custom_TG == null)
            //	{
            //		OnMouseMapMove(mouseInput);
            //	}
            //	//Zoom
            //	else if (mouseInput.scroll.ReadValue() != Vector2.zero && m_custom_TG == null)
            //	{
            //		OnMouseZoomMap(mouseInput);
            //	}
            //}

            if (m_incident_area_content.activeSelf)
            {
                //Do not move the obejct
            }
        }
        #endregion
        #region Mouse Input Related methods
        private void OnMouseMapMove(Mouse mouseInput)
        {
            if (m_camera_2D.gameObject.activeSelf)
            {
                Vector3 deltaOne = OnPlanePositionDelta(mouseInput);
                if (mouseInput.leftButton.isPressed)
                {
                    transform.Translate(deltaOne, Space.Self);
                }
            }
        }
        private void OnMouseZoomMap(Mouse mouseInput)
        {
            if (m_camera_2D.gameObject.activeSelf && mouseInput.scroll.ReadValue() != Vector2.zero)
            {
                //Zoom in
                if (mouseInput.scroll.y.ReadValue() > 0f)
                {
                    m_camera_2D.orthographicSize -= 10;
                }
                //Zoom out
                else if (mouseInput.scroll.y.ReadValue() < 0f)
                {
                    m_camera_2D.orthographicSize += 10;
                }
            }
        }
        private void OnMouseCTG(Mouse mouseInput)
        {
            m_custom_TG = OnMouseHitCTG(mouseInput);
            if (m_custom_TG == null)
            {
                Vector2 screen_pos_3fingers = mouseInput.position.ReadValue();
                Vector3 world_pos_3fingers = OnPlanePositionFromScreenPoint(screen_pos_3fingers);

                m_custom_TG = Instantiate(m_custom_TG_prefab, world_pos_3fingers, Quaternion.Euler(90f, 0f, 0f), null);
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_side_menu.SetActive(true);
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_player = this.gameObject;
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_map = map;
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_camera_2D = m_camera_2D;
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_camera_3D = m_camera_3D;
            }
            else
            {
                Vector3 deltaOne = OnPlanePositionDelta(mouseInput);
                m_custom_TG.transform.Translate(-deltaOne, Space.World);
            }
        }
        private GameObject OnMouseHitCTG(Mouse mouseInput)
        {
            Vector2 screen_pos = mouseInput.position.ReadValue();
            var ray = m_camera_2D.ScreenPointToRay(screen_pos);
            RaycastHit enterNow;
            if (Physics.Raycast(ray, out enterNow))
            {
                if (enterNow.transform.CompareTag("Yellow"))
                {
                    return enterNow.transform.gameObject;
                }
            }
            return null;
        }

        #endregion

        #region Custom tactical graphic
        private void OnCustomTGDrag(Touchscreen touchInput, Vector3 deltaOne, GameObject hit_TG)
        {
            deltaOne = OnPlanePositionDelta(touchInput.touches.ToArray()[0]);
            Vector3 delta_2 = OnPlanePositionDelta(touchInput.touches.ToArray()[1]);
            Vector3 delta_3 = OnPlanePositionDelta(touchInput.touches.ToArray()[2]);

            hit_TG.GetComponent<CustomTacticalGraphics>().m_save_button.SetActive(true);
            hit_TG.GetComponent<CustomTacticalGraphics>().m_delete_button.SetActive(true);

            if ((touchInput.touches.ToArray()[0].phase.ReadValue().Equals(TouchPhase.Moved) || touchInput.touches.ToArray()[1].phase.ReadValue().Equals(TouchPhase.Moved)
                || touchInput.touches.ToArray()[2].phase.ReadValue().Equals(TouchPhase.Moved)) && !es.alreadySelecting && es.currentSelectedGameObject == null)
            {
                hit_TG.transform.Translate(-((deltaOne + delta_2 + delta_3) / 3), Space.World);
                Debug.Log("moving CustomTG:    " + hit_TG.transform.position);
            }
        }
        private GameObject OnCustomTGHit(Touchscreen touchInput)
        {
            Vector2 screen_pos = (touchInput.touches.ToArray()[0].position.ReadValue() + touchInput.touches.ToArray()[1].position.ReadValue() + touchInput.touches.ToArray()[2].position.ReadValue()) / 3;
            var ray = m_camera_2D.ScreenPointToRay(screen_pos);
            RaycastHit enterNow;
            if (Physics.Raycast(ray, out enterNow))
            {
                if (enterNow.transform.CompareTag("Yellow"))
                {
                    return enterNow.transform.gameObject;
                }
            }
            return null;
        }
        private void OnCustomTacticalGraphic(Touchscreen touchInput, Vector3 deltaOne)
        {
            GameObject hit_TG = OnCustomTGHit(touchInput);
            if (hit_TG == null)
            {
                Vector2 screen_pos_3fingers = touchInput.touches.ToArray()[0].position.ReadValue() + touchInput.touches.ToArray()[1].position.ReadValue() + touchInput.touches.ToArray()[2].position.ReadValue();
                Vector3 world_pos_3fingers = OnPlanePositionFromScreenPoint(screen_pos_3fingers / 3);

                m_custom_TG = Instantiate(m_custom_TG_prefab, world_pos_3fingers, Quaternion.Euler(90f, 0f, 0f), null);
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_side_menu.SetActive(true);
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_player = this.gameObject;
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_map = map;
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_camera_2D = m_camera_2D;
                m_custom_TG.GetComponentInChildren<Canvas>().worldCamera = m_camera_2D;
                m_custom_TG.GetComponent<CustomTacticalGraphics>().m_camera_3D = m_camera_3D;
            }
            else
            {
                OnCustomTGDrag(touchInput, deltaOne, hit_TG);
            }

        }
        #endregion

        #region helper methods
        private Vector3 OnPlanePositionDelta(Mouse mouse)
        {
            var ray_enter = m_camera_2D.ScreenPointToRay(mouse.position.ReadValue() - mouse.delta.ReadValue());
            var ray_exit = m_camera_2D.ScreenPointToRay(mouse.position.ReadValue());
            if (_yPlane.Raycast(ray_enter, out var _entered) && _yPlane.Raycast(ray_exit, out var _exited))
            {
                return ray_enter.GetPoint(_entered) - ray_exit.GetPoint(_exited);
            }
            return Vector3.zero;
        }
        private Vector3 OnPlanePositionDelta(TouchControl touch)
        {
            if (touch.phase.ReadValue() != TouchPhase.Moved)
            {
                return Vector3.zero;
            }
            var ray_enter = m_camera_2D.ScreenPointToRay(touch.position.ReadValue() - touch.delta.ReadValue());
            var ray_exit = m_camera_2D.ScreenPointToRay(touch.position.ReadValue());
            if (_yPlane.Raycast(ray_enter, out var _entered) && _yPlane.Raycast(ray_exit, out var _exited))
            {
                return ray_enter.GetPoint(_entered) - ray_exit.GetPoint(_exited);
            }
            return Vector3.zero;
        }
        public Vector3 OnPlanePositionFromScreenPoint(Vector2 screen_pos)
        {
            var ray = m_camera_2D.ScreenPointToRay(screen_pos);

            if (_yPlane.Raycast(ray, out var enterNow))
            {
                return ray.GetPoint(enterNow);
            }
            return Vector3.zero;
        }
        private Mesh SpriteToMesh(Sprite sprite)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i);
            mesh.uv = sprite.uv;
            mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);

            return mesh;
        }
        #endregion

        #region area Shading with pen
        //Create and Paint with Stylus
        private void OnPenInputFillArea(Pen penInput)
        {
            if (penInput.press.isPressed && m_customPen._canDrawNow >= 3 && !m_customPen.m_colorSelected.sprite.name.Contains("notSelected")
                    && !m_customPen.m_sizeSelected.sprite.name.Contains("notSelected") && !m_customPen.m_modeSelected.sprite.name.Contains("notSelected"))
            {
                //Create RenderTex to capture shape + the Container to holder the drawing tiles
                if (m_shadingContainer_temp == null)
                {
                    float _size = gameObject.GetComponentInChildren<Camera>().orthographicSize;
                    m_shadingContainer_temp = Instantiate(m_paintBrushHolder, transform.position, Quaternion.Euler(0f, 180f, 0f));
                    m_shadingContainer_temp.GetComponent<TextNotesForArea>().m_canvas.worldCamera = m_camera_2D;
                    m_shadingContainer_temp.transform.localScale = new Vector3(_size * .2f, _size * .2f, _size * .2f);

                    m_renderTexture = new RenderTexture(save_filled_texture);
                    m_AreaCamera.targetTexture = m_renderTexture;
                }
                else
                {
                    //Draw the painting tiles
                    save_pen_drawing_button.gameObject.SetActive(true);
                    Vector3 pos = OnPlanePositionFromScreenPoint(penInput.position.ReadValue());
                    float _x = pos.x;
                    float _z = pos.z;
                    GameObject dm = Instantiate(m_paintBrush, new Vector3(_x, 1f, _z), Quaternion.Euler(90f, 0f, 0f), m_shadingContainer_temp.transform);
                    dm.transform.localScale = new Vector3(m_customPen.m_lineSize, m_customPen.m_lineSize, m_customPen.m_lineSize);
                    dm.transform.localScale /= 50;
                    dm.GetComponent<SpriteRenderer>().material.color = m_customPen.m_fillColor;
                }
            }
        }

        //Save shape to png and assign it to a new Obj
        private IEnumerator OnSaveFillPenChanges()
        {
            yield return new WaitForEndOfFrame();
            RenderTexture.active = m_renderTexture;
            Texture2D texture = new Texture2D(save_filled_texture.width, save_filled_texture.height);
            texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture.Apply();
            var data = texture.EncodeToPNG();
            //Saving as a PNG to visualize later
            String path = Application.dataPath + "/area_marking" + Time.deltaTime.ToString() + ".png";
            System.IO.File.WriteAllBytes(path, data);
            //Apply enerated texture to material
            m_shadingContainer_temp.GetComponent<MeshRenderer>().material.mainTexture = texture;
            //Delete all tiles
            StartCoroutine(OnDeletePaintingBrush());
        }
        //Delete the drawing parts and initialize everything else
        private IEnumerator OnDeletePaintingBrush()
        {
            yield return new WaitForEndOfFrame();
            foreach (Transform tr in m_shadingContainer_temp.transform)
            {
                if (tr.gameObject.GetComponent<Canvas>() == null && tr.gameObject.GetComponent<TextMeshProUGUI>() == null)
                {
                    Destroy(tr.gameObject);
                }
            }
            //Init the button in Drop Menu of areas
            button = Instantiate(m_incident_button, m_incident_area_content_content.transform);
            button.GetComponent<MarkedAreaButtonBrhavior>().m_area = m_shadingContainer_temp;
            button.GetComponent<MarkedAreaButtonBrhavior>().m_player = this.gameObject;
            button.GetComponent<MarkedAreaButtonBrhavior>().m_camera_2D = m_camera_2D;
            button.GetComponent<MarkedAreaButtonBrhavior>().m_content = m_incident_area_content;
            button.GetComponent<MarkedAreaButtonBrhavior>().m_back_button = m_back_button_Area;
            button.GetComponent<MarkedAreaButtonBrhavior>().m_shading = m_shadingContainer_temp;

            //Associate
            m_shadingContainer_temp.GetComponent<PaintBrush>().map = map;
            m_shadingContainer_temp.GetComponent<PaintBrush>().m_button = button;

            //Clear
            m_shadingContainer_temp.transform.SetParent(null);
            float _x = m_shadingContainer_temp.transform.position.x;
            float _z = m_shadingContainer_temp.transform.position.z;
            m_shadingContainer_temp.transform.position = new Vector3(_x, 8f, _z);
            m_incident_areas_List.Add(m_shadingContainer_temp);
            m_shadingContainer_temp = null;
            StartCoroutine(OnSevaeContour());
        }
        #endregion

        #region area Marking & Shading with finger
        private void OnFingerInputFillArea(Touchscreen touchInput)
        {
            if (touchInput.touches.ToArray()[0].isInProgress)
            {
                //Create RenderTex to capture shape + the Container to holder the drawing tiles
                if (m_shadingContainer_temp == null)
                {
                    float _size = gameObject.GetComponentInChildren<Camera>().orthographicSize;
                    m_shadingContainer_temp = Instantiate(m_paintBrushHolder, transform.position, Quaternion.Euler(0f, 180f, 0f));
                    m_shadingContainer_temp.GetComponent<TextNotesForArea>().m_2DCamera = m_camera_2D;
                    m_shadingContainer_temp.transform.localScale = new Vector3(_size * .2f, _size * .2f, _size * .2f);

                    m_renderTexture = new RenderTexture(save_filled_texture);
                    m_AreaCamera.targetTexture = m_renderTexture;
                }
                else
                {
                    //Draw the painting tiles
                    save_pen_drawing_button.gameObject.SetActive(true);
                    Vector3 pos = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue());
                    float _x = pos.x;
                    float _z = pos.z;
                    GameObject dm = Instantiate(m_paintBrush, new Vector3(_x, 1f, _z), Quaternion.Euler(90f, 0f, 0f), m_shadingContainer_temp.transform);
                    dm.transform.localScale = new Vector3(m_customPen.m_lineSize, m_customPen.m_lineSize, m_customPen.m_lineSize);
                    dm.transform.localScale /= 50;
                    dm.GetComponent<SpriteRenderer>().material.color = m_customPen.m_fillColor;
                }
            }
        }

        private void OnFingerInputDraw(Touchscreen touchInput)
        {

            Vector3 world_pos = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue());

            if (touchInput.touches.ToArray()[0].isInProgress)
            {
                if (curr_brush_tile == null)
                {
                    world_pos = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue());
                    OnInitLineNew(world_pos);
                }
                save_pen_drawing_button.gameObject.SetActive(true);
                world_pos = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue());
                OnDrawLineNew(world_pos);
            }
        }

        #endregion

        #region area marking with pen

        private void OnInitLineNew(Vector3 pos)
        {

            curr_brush_tile = Instantiate(m_line_brush, pos, Quaternion.Euler(0f, 0f, 0f));
            curr_brush_tile.GetComponent<PaintBrush>().map = map;

            //Set Up custom line
            curr_brush_tile.GetComponent<LineRenderer>().materials[0].color = m_customPen.m_lineColor;
            curr_brush_tile.GetComponent<LineRenderer>().startWidth = m_customPen.m_lineSize;
            curr_brush_tile.GetComponent<LineRenderer>().endWidth = m_customPen.m_lineSize;


            curr_brush_tile.transform.position = new Vector3(curr_brush_tile.transform.position.x, 8f, curr_brush_tile.transform.position.z);
            m_line_renderer = curr_brush_tile.GetComponent<LineRenderer>();
            brush_points.Clear();
            brush_points.Add(new Vector3(pos.x, 8f, pos.z));
            brush_points.Add(new Vector3(pos.x, 8f, pos.z));
            m_line_renderer.SetPosition(0, brush_points[0]);
            m_line_renderer.SetPosition(1, brush_points[1]);
        }
        private void OnDrawLineNew(Vector3 pos)
        {
            brush_points.Add(new Vector3(pos.x, 8f, pos.z));
            m_line_renderer.positionCount++;
            m_line_renderer.SetPosition(m_line_renderer.positionCount - 1, new Vector3(pos.x, 8f, pos.z));
        }

        private void OnPenInputDraw(Pen penInput)
        {

            Vector3 world_pos = OnPlanePositionFromScreenPoint(penInput.position.ReadValue());

            if (penInput.press.isPressed)
            {
                if (curr_brush_tile == null)
                {
                    world_pos = OnPlanePositionFromScreenPoint(penInput.position.ReadValue());
                    OnInitLineNew(world_pos);
                }
                save_pen_drawing_button.gameObject.SetActive(true);
                world_pos = OnPlanePositionFromScreenPoint(penInput.position.ReadValue());
                OnDrawLineNew(world_pos);
            }
        }

        public IEnumerator OnSevaeContour()
        {
            yield return new WaitForEndOfFrame();
            Vector3 _pos = curr_brush_tile.transform.position;
            curr_brush_tile.tag = "Area";
            curr_brush_tile.layer = 0;
            if (m_shadingContainer_temp != null)
            {
                StartCoroutine(OnSaveFillPenChanges());
            }
            brush_points.RemoveAt(brush_points.Count - 1);
            curr_brush_tile.GetComponent<LineRenderer>().SetPositions(brush_points.ToArray());

            if (button != null)
            {
                button.GetComponent<MarkedAreaButtonBrhavior>().m_contour = curr_brush_tile;
                curr_brush_tile.transform.SetParent(null);
                button = null;
                m_incident_line_List.Add(curr_brush_tile);
                curr_brush_tile = null;
                save_pen_drawing_button.gameObject.SetActive(false);
                m_customPen.SetDrawWithFinger();
            }
            else
            {
                Debug.Log("Please Shade the inside of the area");
                if (!m_incident_line_List.Contains(curr_brush_tile))
                {
                    m_incident_line_List.Add(curr_brush_tile);
                }
            }
        }
        #endregion

        #region 2D camera related methods
        private void OnMoveCamera2D(Touchscreen touchInput, Vector3 deltaOne, Transform m_transform)
        {
            Vector2 screen_pos = Vector2.zero;

            //First Touch of screen
            if (touchInput.touches.ToArray()[0].phase.ReadValue().Equals(TouchPhase.Began))
            {
                touch_time = Time.time;
                prev_pos = m_transform.position;
                screen_pos = touchInput.touches.ToArray()[0].position.ReadValue();

                var ray = m_camera_2D.ScreenPointToRay(screen_pos);
                RaycastHit enterNow;
                if (Physics.Raycast(ray, out enterNow))
                {
                    if (enterNow.transform.CompareTag("Red"))
                    {
                        m_hit_sticker = true;
                        this.hit = enterNow.transform.gameObject;
                    }
                    else if (enterNow.transform.CompareTag("Yellow"))
                    {
                        m_custom_TG = enterNow.transform.gameObject;
                        m_custom_TG.GetComponent<CustomTacticalGraphics>().m_save_button.SetActive(true);
                        m_custom_TG.GetComponent<CustomTacticalGraphics>().m_delete_button.SetActive(true);
                    }
                }
                else if (m_custom_TG == null)
                {
                    touch_point.transform.position = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue());
                }
            }
            deltaOne = OnPlanePositionDelta(touchInput.touches.ToArray()[0]);

            //Move Sticker
            if (m_hit_sticker && m_custom_TG == null)
            {
                if (hit != null)
                {
                    this.hit.GetComponent<IconCorrectView>().validate_button.gameObject.SetActive(true);
                    this.hit.GetComponent<IconCorrectView>().delete_button.gameObject.SetActive(true);
                    OnTaktischeStickerEdit(touchInput, deltaOne, hit);
                }
                else
                {
                    m_hit_sticker = false;
                }

            }

            //Move Map
            else if (!m_hit_sticker && m_custom_TG == null && !es.alreadySelecting && es.currentSelectedGameObject == null && !m_tactical_folder.activeSelf)
            {
                if (touchInput.touches.ToArray()[0].isInProgress && touchInput.touches.ToArray()[0].phase.ReadValue().Equals(TouchPhase.Moved))
                {
                    m_transform.Translate(deltaOne, Space.Self);
                    transform.position = Vector3.SmoothDamp(transform.position, m_transform.position, ref refVelocity, m_smooth_speed * 20f * Time.deltaTime);
                }

            }

            //Move Custom TG
            else if (m_custom_TG != null && !m_custom_TG.GetComponent<CustomTacticalGraphics>().m_side_menu.activeSelf)
            {
                deltaOne = OnPlanePositionDelta(touchInput.touches.ToArray()[0]);
                if (touchInput.touches.ToArray()[0].phase.ReadValue().Equals(TouchPhase.Moved))
                {
                    m_custom_TG.transform.Translate(-deltaOne, Space.World);
                }
            }
        }
        private void OnZoomCamera2D(Touchscreen touchInput)
        {


            var pos_fingerOne_enter = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue());
            var pos_fingerTwo_enter = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[1].position.ReadValue());

            var pos_fingerOne_exit = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue() - touchInput.touches.ToArray()[0].delta.ReadValue());
            var pos_fingerTwo_exit = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[1].position.ReadValue() - touchInput.touches.ToArray()[1].delta.ReadValue());

            //Zooming
            var zoom_factor = Vector3.Distance(pos_fingerOne_enter, pos_fingerTwo_enter) / Vector3.Distance(pos_fingerOne_exit, pos_fingerTwo_exit);
            if (zoom_factor == 0 || zoom_factor > 10)
            {
                return;
            }
            m_camera_2D_height -= (zoom_factor * 100f) - 100f;
            if (m_camera_2D_height <= 10f)
            {
                m_camera_2D_height = 10f;
            }
            if (m_camera_2D_height >= 2000f)
            {
                m_camera_2D_height = 2000f;
            }
            m_camera_2D.orthographicSize = m_camera_2D_height;
            //Here to Dynamically Increase/decrease the size --> still not supported
            //tile size and buffer size should be set at the start ONLY
            if (m_camera_2D.orthographicSize < 300)
            {
                map.Options.extentOptions.defaultExtents.rangeAroundTransformOptions.visibleBuffer = 8;
                map.Options.extentOptions.defaultExtents.rangeAroundTransformOptions.disposeBuffer = 30;
            }
            else if (m_camera_2D.orthographicSize >= 300 && m_camera_2D.orthographicSize < 800)
            {
                map.Options.extentOptions.defaultExtents.rangeAroundTransformOptions.visibleBuffer = 16;
                map.Options.extentOptions.defaultExtents.rangeAroundTransformOptions.disposeBuffer = 120;
            }
            else
            {
                map.Options.extentOptions.defaultExtents.rangeAroundTransformOptions.visibleBuffer = 24;
                map.Options.extentOptions.defaultExtents.rangeAroundTransformOptions.disposeBuffer = 180;
            }
        }




        #endregion

        #region 3D camera related methods
        private void OnRotateCamera3D(Touchscreen touchInput)
        {
            var pos_fingerOne_enter = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue());
            var pos_fingerTwo_enter = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[1].position.ReadValue());

            var pos_fingerOne_exit = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue() - touchInput.touches.ToArray()[0].delta.ReadValue());
            var pos_fingerTwo_exit = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[1].position.ReadValue() - touchInput.touches.ToArray()[1].delta.ReadValue());

            if (pos_fingerTwo_exit != pos_fingerTwo_enter)
            {
                float _angle = Vector3.SignedAngle(pos_fingerTwo_enter - pos_fingerOne_enter, pos_fingerTwo_exit - pos_fingerOne_exit, Vector3.up);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(0f, _angle, 0f) + transform.localRotation.eulerAngles), m_smooth_speed);

            }
        }
        private void OnMoveCamera3D(Touchscreen touchInput, Vector3 deltaOne, Transform m_transform)
        {
            m_transform = transform;
            deltaOne = OnPlanePositionDelta(touchInput.touches.ToArray()[0]);

            if (touchInput.touches.ToArray()[0].phase.ReadValue().Equals(TouchPhase.Moved))
            {
                m_transform.Translate(deltaOne, Space.World);
            }
            if (touchInput.touches.ToArray()[0].phase.ReadValue().Equals(TouchPhase.Ended))
            {
                transform.position = Vector3.SmoothDamp(transform.position, m_transform.position, ref refVelocity, m_smooth_speed * Time.deltaTime);

            }
        }
        #endregion

        #region tactical Graphics related methods
        public void SpawnTaKTischePrefOnButtonPress(int button_id)
        {
            pref = Instantiate(m_dragTG, this.touch_point.transform.position, Quaternion.Euler(0f, 0f, 0f));

            pref.GetComponent<SpriteRenderer>().sprite = taktische_zeichnen[button_id];
            pref.transform.SetParent(null, true);
            pref.GetComponent<IconCorrectView>().m_target = this.transform;
            pref.GetComponent<IconCorrectView>().m_camera_2D = m_camera_2D;
            pref.GetComponent<IconCorrectView>().m_camera_3D = m_camera_3D;
            pref.GetComponentInChildren<Canvas>().worldCamera = m_camera_2D;
            pref.GetComponent<IconCorrectView>().validate_button.gameObject.SetActive(false);
            pref.GetComponent<IconCorrectView>().delete_button.gameObject.SetActive(false);
            pref.GetComponent<IconCorrectView>().map = this.map;
            pref.GetComponent<IconCorrectView>().dynamic_latLang = map.WorldToGeoPosition(this.touch_point.transform.position);
            pref.GetComponent<IconCorrectView>().size_of_wagon = 1;
            pref.GetComponent<IconCorrectView>().fire_truck = m_fire_truck;
            pref.GetComponent<IconCorrectView>().touch_point = this.touch_point.transform.position;

        }
        private void OnTaktischeStickerEdit(Touchscreen touchInput, Vector3 deltaOne, GameObject hit)
        {
            Transform hit_temp = this.hit.transform;
            deltaOne = OnPlanePositionDelta(touchInput.touches.ToArray()[0]);
            if (touchInput.touches.ToArray()[0].phase.ReadValue().Equals(TouchPhase.Moved))
            {
                hit_temp.Translate(-deltaOne, Space.World);
            }
            if (touchInput.touches.ToArray()[0].phase.ReadValue().Equals(TouchPhase.Ended))
            {
                this.hit.transform.position = Vector3.SmoothDamp(this.hit.transform.position, hit_temp.position, ref refVelocity, m_smooth_speed * Time.deltaTime);
            }


        }
        public void OnEditClose()
        {
            m_hit_sticker = false;
        }
        #endregion

        #region button behavior related methods
        public void OnOpenAreaMenu()
        {
            m_incident_area_content.SetActive(true);
            m_back_button_Area.SetActive(true);
        }
        public void OnPenSaveChanges()
        {
            switch (m_customPen.m_drawMode)
            {
                case 0:
                    StartCoroutine(OnSevaeContour());
                    save_pen_drawing_button.gameObject.SetActive(false);
                    break;
                case 1:
                    StartCoroutine(OnSaveFillPenChanges());
                    save_pen_drawing_button.gameObject.SetActive(false);
                    break;
            }

        }
        #endregion



        //#region deprecated-code
        //private void OnRotateCamera2D(Touchscreen touchInput)
        //{
        //	var pos_fingerOne_enter = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue());
        //	var pos_fingerTwo_enter = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[1].position.ReadValue());

        //	var pos_fingerOne_exit = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[0].position.ReadValue() - touchInput.touches.ToArray()[0].delta.ReadValue());
        //	var pos_fingerTwo_exit = OnPlanePositionFromScreenPoint(touchInput.touches.ToArray()[1].position.ReadValue() - touchInput.touches.ToArray()[1].delta.ReadValue());

        //	if (pos_fingerTwo_exit != pos_fingerTwo_enter)
        //	{
        //		float _angle = Vector3.SignedAngle(pos_fingerTwo_enter - pos_fingerOne_enter, pos_fingerTwo_exit - pos_fingerOne_exit, Vector3.up);

        //		//_yPlane.normal instead of vector3.up
        //		//move_target.LookAt(move_target);
        //		transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(0f, _angle, 0f) + transform.localRotation.eulerAngles), m_smooth_speed);

        //	}
        //}
        //private IEnumerator DeleteBrushTiles(string path)
        //{
        //	yield return new WaitForSeconds(3f);
        //	byte[] data = System.IO.File.ReadAllBytes(path);
        //	Texture2D texture = new Texture2D(save_drawing_texture.width, save_drawing_texture.height, TextureFormat.RGBA32, false);
        //	texture.LoadImage(data);
        //	buffer.GetComponent<BufferSetPosition>().center = FindCenteroid(buffer);
        //	foreach (Transform child in buffer.transform)
        //	{
        //		Destroy(child.gameObject);
        //	}
        //	buffer.GetComponent<MeshRenderer>().enabled = true;
        //	Material material = buffer.GetComponent<MeshRenderer>().material;
        //	if (material == null)
        //	{
        //		Debug.Log("material is null from MeshRenderer");
        //	}
        //	else
        //	{
        //		buffer.GetComponent<MeshRenderer>().material.mainTexture = texture;
        //	}
        //	buffer.transform.SetParent(null, true);
        //	buffer = Instantiate(m_brush_buffer, this.transform.position, Quaternion.identity, this.transform);
        //	buffer.GetComponent<BufferSetPosition>().center = this.transform.position;
        //}
        //private void OnDrawLine(Vector3 pos)
        //{
        //	save_pen_drawing_button.gameObject.SetActive(true);
        //	Vector3 final_pos = Vector3.Scale(pos, new Vector3(1f, 0f, 1f));
        //	final_pos.y = map.transform.position.y + .2f;
        //	GameObject tile = Instantiate(m_brush, final_pos, Quaternion.Euler(0f, 0f, 0f));
        //	tile.transform.SetParent(buffer.transform, true);
        //}
        //private void OnDeleteLine(Pen penInput)
        //{
        //	Vector2 pen_screen_pos = penInput.position.ReadValue();
        //	var pen_ray = m_camera_2D.ScreenPointToRay(pen_screen_pos);
        //	RaycastHit enterNow;
        //	if (Physics.Raycast(pen_ray, out enterNow))
        //	{
        //		if (enterNow.transform.CompareTag("Brush"))
        //		{
        //			Destroy(enterNow.transform.gameObject);
        //		}
        //	}
        //}
        //private IEnumerator OnFillArea()
        //{
        //	yield return new WaitForEndOfFrame();
        //	//Create customized Mesh
        //	//Fill this Mesh with transparent material 
        //	filled_area.GetComponent<MeshRenderer>().enabled = true;
        //	Material material = filled_area.GetComponent<MeshRenderer>().material;
        //	if (material == null)
        //	{
        //		Debug.Log("material is null from MeshRenderer");
        //	}
        //	else
        //	{
        //		//TODO Mesh Creator correction
        //		//MeshCreator(filled_area,brush_points.ToArray());
        //	}

        //	filled_area.GetComponent<MeshRenderer>().enabled = true;
        //	filled_area = null;
        //}
        //private Vector3 FindCenteroid(Vector3[] points)
        //{
        //	Vector3 _pos = Vector3.zero;
        //	foreach (Vector3 popo in points)
        //	{
        //		_pos += popo;
        //	}
        //	return _pos / points.Length;
        //}
        //private Mesh MeshCreator(Vector3[] setOfVertices)
        //{
        //	Vector3[] filledGraphPoints = new Vector3[setOfVertices.Length * 2];
        //	for (int j = 0; j < setOfVertices.Length; ++j)
        //	{
        //		filledGraphPoints[2 * j] = new Vector3(setOfVertices[j].x, 0, 0);
        //		filledGraphPoints[2 * j + 1] = setOfVertices[j];
        //	}
        //	int numTriangles = (setOfVertices.Length - 1) * 2;
        //	int[] triangles = new int[numTriangles * 3];
        //	int i = 0;
        //	for (int t = 0; t < numTriangles; t += 2)
        //	{
        //		triangles[i++] = 2 * t;
        //		triangles[i++] = 2 * t + 1;
        //		triangles[i++] = 2 * t + 2;

        //		triangles[i++] = 2 * t + 1;
        //		triangles[i++] = 2 * t + 2;
        //		triangles[i++] = 2 * t + 3;
        //	}

        //	// create mesh
        //	Mesh filledGraphMesh = new Mesh();
        //	filledGraphMesh.vertices = filledGraphPoints;
        //	filledGraphMesh.triangles = triangles;
        //	return filledGraphMesh;
        //}


        //private IEnumerator OnGenerateFillingShape()
        //{
        //    //Generate RenderTexture
        //    m_renderTexture = new RenderTexture(save_filled_texture);
        //    m_AreaCamera.targetTexture = m_renderTexture;

        //    //Generate the image data
        //    yield return new WaitForEndOfFrame();
        //    RenderTexture.active = m_renderTexture;
        //    Texture2D texture = new Texture2D(save_filled_texture.width, save_filled_texture.height);
        //    texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        //    texture.Apply();
        //    var data = texture.EncodeToPNG();

        //    //Saving as a PNG
        //    String path = Application.dataPath + "/area_marking" + Time.deltaTime.ToString() + ".png";
        //    System.IO.File.WriteAllBytes(path, data);

        //    //Create a sprite from the loaded image
        //    texture.alphaIsTransparency = true;
        //    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, save_filled_texture.width, save_filled_texture.height), new Vector2(0f, 0f));

        //    //Init the button in Drop Menu of areas
        //    button = Instantiate(m_incident_button, m_incident_area_content_content.transform);
        //    button.GetComponent<MarkedArea>().m_area = curr_brush_tile;
        //    button.GetComponent<MarkedArea>().m_player = this.gameObject;
        //    button.GetComponent<MarkedArea>().m_camera_2D = m_camera_2D;
        //    button.GetComponent<MarkedArea>().m_content = m_incident_area_content;
        //    button.GetComponent<MarkedArea>().m_back_button = m_back_button_Area;

        //    //Create a mesh from the loaded image & apply it to the object
        //    Mesh mesh = SpriteToMesh(sprite);
        //    filled_area.GetComponent<MeshFilter>().mesh = mesh;
        //    filled_area.GetComponent<MeshFilter>().sharedMesh = mesh;
        //    filled_area.GetComponent<MeshCollider>().sharedMesh = mesh;

        //    //save area in list and add to  drawn region
        //    m_incident_areas.Add(curr_brush_tile);
        //    curr_brush_tile.GetComponent<PaintBrush>().m_area_button = button;
        //    curr_brush_tile.GetComponent<PaintBrush>().m_filled_region = filled_area;
        //    curr_brush_tile.tag = "Area";
        //    filled_area.tag = "Area";
        //    filled_area.layer = 0;
        //    curr_brush_tile.layer = 0;
        //    //make independant 
        //    curr_brush_tile.transform.SetParent(null, true);

        //    ////positioning --> Z position TODO
        //    //Bounds m_bounds = filled_area.GetComponentInChildren<MeshFilter>().mesh.bounds;
        //    //Vector3 offset = filled_area.transform.position - filled_area.transform.TransformPoint(m_bounds.center);
        //    //Debug.Log("pos:  " + filled_area.transform.position);
        //    //filled_area.transform.position = new Vector3(filled_area.transform.position.x + offset.x, filled_area.transform.position.y, -640f);

        //    //Clear Variables
        //    curr_brush_tile = null;
        //    filled_area = null;
        //    button = null;
        //}

        ////Instantiate the shaded region
        //private IEnumerator OnCreateFillingTile()
        //{
        //    yield return new WaitForEndOfFrame();
        //    if (curr_brush_tile == null)
        //    {
        //        Debug.Log("curr_brush is null");
        //    }
        //    else
        //    {
        //        //curr_brush_tile.GetComponent<LineRenderer>().Simplify(5f);
        //        if (filled_area == null)
        //        {

        //            filled_area = Instantiate(m_filling_brush, new Vector3(curr_brush_tile.transform.position.x, 0f, curr_brush_tile.transform.position.z), Quaternion.Euler(0f, 180f, 0f));
        //            filled_area.GetComponent<FilledArea>().m_camera_2D = m_camera_2D;
        //            filled_area.GetComponent<FilledArea>().m_map = map;
        //            filled_area.GetComponentInChildren<Canvas>().worldCamera = m_camera_2D;
        //            filled_area.GetComponent<FilledArea>().m_line_renderer = curr_brush_tile.GetComponent<LineRenderer>();
        //            StartCoroutine(OnGenerateFillingShape());
        //        }
        //    }

        //}
        //#endregion
    }
}