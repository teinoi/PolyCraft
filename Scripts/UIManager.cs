using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ProBuilder;

/// <summary>
/// 모든 UI 버튼과 기능 스크립트를 연결하는 메인 UI 컨트롤러
/// </summary>
public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public struct ShapeButton
    {
        public Button button;
        public ShapeType shapeType;
    }

    // 도형 색성 및 조작 버튼
    public ShapeButton[] shapeButtons;
    public Button createModeButton;
    public Button copyButton;
    public Button combineButton;
    public Button deleteButton;

    // 머티리얼 관련 버튼
    public Button[] colorButtons;
    public Button materialButton;

    // 이동 조작 버튼
    public Button moveButton;
    public Button moveXButton;
    public Button moveYButton;
    public Button moveZButton;

    // 회전 조작 버튼
    public Button rotateButton;
    public Button rotateXButton;
    public Button rotateYButton;
    public Button rotateZButton;

    // 스케일 조작 버튼
    public Button scaleButton;
    public Button scaleXButton;
    public Button scaleYButton;
    public Button scaleZButton;

    public delegate void ShapeSelectedHandler(ShapeType shapeType);
    public static event ShapeSelectedHandler OnShapeSelected;

    // 기능 클래스 참조
    public MeshCreate meshCreate;
    public MeshCopy meshCopy;
    public MeshCombiner meshCombiner;
    public MeshDelete meshDelete;
    public MeshMaterial meshMaterial;

    public MeshTransform meshTransform;
    public MeshMove meshMove;
    public MeshScale meshScale;
    public MeshRotate meshRotate;

    // 마지막으로 선택된 도형과 색상
    ShapeType lastSelectedShapeType; 
    Color lastSelectedColor = Color.white;

    void Start()
    {
        #region Create Shape
        if (createModeButton != null)
        {
            createModeButton.onClick.AddListener(meshCreate.ToggleCreateMode);
        }

        foreach (var shapeButton in shapeButtons)
        {
            shapeButton.button.onClick.AddListener(() => SelectShape(shapeButton.shapeType));
        }
        #endregion

        #region Copy
        if (copyButton != null)
        {
            copyButton.onClick.AddListener(meshCopy.DuplicateMeshesWithOutline);
        }
        #endregion

        #region Combine
        if (combineButton != null)
        {
            combineButton.onClick.AddListener(meshCombiner.CombineMeshesWithOutline);
        }
        #endregion

        #region Delete
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(meshDelete.DeleteMeshesWithOutline);
        }
        #endregion

        #region Material
        foreach (Button colorButton in colorButtons)
        {
            colorButton.onClick.AddListener(() => ApplyButtonColor(colorButton));
        }

        if (materialButton != null)
        {
            materialButton.onClick.AddListener(ApplyLastSelectedMaterial);
        }
        #endregion

        #region Move
        if (moveButton != null)
        {
            moveButton.onClick.AddListener(() =>
            {
                meshMove.enabled = true;
                meshRotate.enabled = false;
                meshScale.enabled = false;

                meshTransform.MoveButton();
            });
        }

        if (moveXButton != null)
        {
            moveXButton.onClick.AddListener(() =>
            {
                meshMove.enabled = true;
                meshRotate.enabled = false;
                meshScale.enabled = false;

                meshMove.StartMoving(MeshMove.Axis.X);
            });
        }

        if (moveYButton != null)
        {
            moveYButton.onClick.AddListener(() =>
            {
                meshMove.enabled = true;
                meshRotate.enabled = false;
                meshScale.enabled = false;

                meshMove.StartMoving(MeshMove.Axis.Y);
            });
        }

        if (moveZButton != null)
        {
            moveZButton.onClick.AddListener(() =>
            {
                meshMove.enabled = true;
                meshRotate.enabled = false;
                meshScale.enabled = false;

                meshMove.StartMoving(MeshMove.Axis.Z);
            });
        }
        #endregion

        #region Rotate
        if (rotateButton != null)
        {
            rotateButton.onClick.AddListener(() =>
            {
                meshRotate.enabled = true;
                meshMove.enabled = false;
                meshScale.enabled = false;

                meshTransform.RotateButton();
            });
        }

        if (rotateXButton != null)
        {
            rotateXButton.onClick.AddListener(() =>
            {
                meshRotate.enabled = true;
                meshMove.enabled = false;
                meshScale.enabled = false;

                meshRotate.StartRotating(MeshRotate.Axis.X);
            });
        }

        if (rotateYButton != null)
        {
            rotateYButton.onClick.AddListener(() =>
            {
                meshRotate.enabled = true;
                meshMove.enabled = false;
                meshScale.enabled = false;

                meshRotate.StartRotating(MeshRotate.Axis.Y);
            });
        }

        if (rotateZButton != null)
        {
            rotateZButton.onClick.AddListener(() =>
            {
                meshRotate.enabled = true;
                meshMove.enabled = false;
                meshScale.enabled = false;

                meshRotate.StartRotating(MeshRotate.Axis.Z);
            });
        }
        #endregion

        #region Scale
        if (scaleButton != null)
        {
            scaleButton.onClick.AddListener(() =>
            {
                meshScale.enabled = true;
                meshMove.enabled = false;
                meshRotate.enabled = false;

                meshScale.StartScaling(MeshScale.Axis.All);
            });
        }

        if (scaleXButton != null)
        {
            scaleXButton.onClick.AddListener(() =>
            {
                meshScale.enabled = true;
                meshMove.enabled = false;
                meshRotate.enabled = false;

                meshScale.StartScaling(MeshScale.Axis.X);
            });
        }

        if (scaleYButton != null)
        {
            scaleYButton.onClick.AddListener(() =>
            {
                meshScale.enabled = true;
                meshMove.enabled = false;
                meshRotate.enabled = false;

                meshScale.StartScaling(MeshScale.Axis.Y);
            });
        }

        if (scaleZButton != null)
        {
            scaleZButton.onClick.AddListener(() =>
            {
                meshScale.enabled = true;
                meshMove.enabled = false;
                meshRotate.enabled = false;

                meshScale.StartScaling(MeshScale.Axis.Z);
            });
        }
        #endregion
    }

    // 마지막으로 선택된 셰이프 저장
    void SelectShape(ShapeType shapeType)
    {
        lastSelectedShapeType = shapeType;
        OnShapeSelected?.Invoke(shapeType);
    }

    // 마지막으로 선택된 색상 저장
    void ApplyButtonColor(Button button)
    {
        Color buttonColor = button.GetComponent<Image>().color;
        lastSelectedColor = buttonColor;

        if (meshMaterial != null)
        {
            meshMaterial.SetColor(buttonColor);
            meshMaterial.StartApplying();
        }
    }

    // 마지막으로 선택된 색상 적용
    void ApplyLastSelectedMaterial()
    {
        if (meshMaterial != null)
        {
            meshMaterial.SetColor(lastSelectedColor);
            meshMaterial.StartApplying();
        }
    }
}