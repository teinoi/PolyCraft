using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ProBuilder;

/// <summary>
/// ��� UI ��ư�� ��� ��ũ��Ʈ�� �����ϴ� ���� UI ��Ʈ�ѷ�
/// </summary>
public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public struct ShapeButton
    {
        public Button button;
        public ShapeType shapeType;
    }

    // ���� ���� �� ���� ��ư
    public ShapeButton[] shapeButtons;
    public Button createModeButton;
    public Button copyButton;
    public Button combineButton;
    public Button deleteButton;

    // ��Ƽ���� ���� ��ư
    public Button[] colorButtons;
    public Button materialButton;

    // �̵� ���� ��ư
    public Button moveButton;
    public Button moveXButton;
    public Button moveYButton;
    public Button moveZButton;

    // ȸ�� ���� ��ư
    public Button rotateButton;
    public Button rotateXButton;
    public Button rotateYButton;
    public Button rotateZButton;

    // ������ ���� ��ư
    public Button scaleButton;
    public Button scaleXButton;
    public Button scaleYButton;
    public Button scaleZButton;

    public delegate void ShapeSelectedHandler(ShapeType shapeType);
    public static event ShapeSelectedHandler OnShapeSelected;

    // ��� Ŭ���� ����
    public MeshCreate meshCreate;
    public MeshCopy meshCopy;
    public MeshCombiner meshCombiner;
    public MeshDelete meshDelete;
    public MeshMaterial meshMaterial;

    public MeshTransform meshTransform;
    public MeshMove meshMove;
    public MeshScale meshScale;
    public MeshRotate meshRotate;

    // ���������� ���õ� ������ ����
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

    // ���������� ���õ� ������ ����
    void SelectShape(ShapeType shapeType)
    {
        lastSelectedShapeType = shapeType;
        OnShapeSelected?.Invoke(shapeType);
    }

    // ���������� ���õ� ���� ����
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

    // ���������� ���õ� ���� ����
    void ApplyLastSelectedMaterial()
    {
        if (meshMaterial != null)
        {
            meshMaterial.SetColor(lastSelectedColor);
            meshMaterial.StartApplying();
        }
    }
}