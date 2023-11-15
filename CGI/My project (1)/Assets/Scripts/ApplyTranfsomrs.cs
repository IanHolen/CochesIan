// Codigo por Ian Holender 15/11/2023


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTransforms : MonoBehaviour
{
    [SerializeField] Vector3 displacement;
    [SerializeField]AXIS rotationAxis;

    // Se definen las llantas como objeto de juego para poder llamarlas dentro de unity. 
    [SerializeField] GameObject wheel1;
    [SerializeField] GameObject wheel2;
    [SerializeField] GameObject wheel3;
    [SerializeField] GameObject wheel4;

    
    Mesh mesh;

    //Se generan un nuevo mesh para la llanta 1 
    Mesh wheel1mesh;

    //Se generan un nuevo mesh para la llanta 2
    Mesh wheel2mesh;

    //Se generan un nuevo mesh para la llanta 3
    Mesh wheel3mesh;

    //Se generan un nuevo mesh para la llanta 4
    Mesh wheel4mesh;

    Vector3[] baseVertices;
    Vector3[] newVertices;

    // Se generan los nuevos vectores donde se guardaran los vertices de cada llanta 
    // Vectores llanta 1
    Vector3[] baseVertices1;
    Vector3[] newVertices1;

     // Vectores llanta 2
    Vector3[] baseVertices2;
    Vector3[] newVertices2;

     // Vectores llanta 3
    Vector3[] baseVertices3;
    Vector3[] newVertices3;

     // Vectores llanta 4
    Vector3[] baseVertices4;
    Vector3[] newVertices4;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh;

        //Se declara como gabe object para poder llamarlo dentro de unity
        wheel1mesh= wheel1.GetComponentInChildren<MeshFilter>().mesh;
        wheel2mesh= wheel2.GetComponentInChildren<MeshFilter>().mesh;
        wheel3mesh= wheel3.GetComponentInChildren<MeshFilter>().mesh;
        wheel4mesh= wheel4.GetComponentInChildren<MeshFilter>().mesh;



        baseVertices = mesh.vertices;

        // Se declaran los vertices base de cada llanta
        baseVertices1 = wheel1mesh.vertices;
        baseVertices2 = wheel2mesh.vertices;
        baseVertices3 = wheel3mesh.vertices;
        baseVertices4 = wheel4mesh.vertices;

       
       


        newVertices = new Vector3[baseVertices.Length];
      
        // Se define el valor de los nuevos vertices de la llanta 1 
        newVertices1 = new Vector3[baseVertices1.Length];
            
        // Se define el valor de los nuevos vertices de la llanta 2
        newVertices2 = new Vector3[baseVertices2.Length];
       
        // Se define el valor de los nuevos vertices de la llanta 3
        newVertices3 = new Vector3[baseVertices3.Length];
       
        // Se define el valor de los nuevos vertices de la llanta 4
        newVertices4 = new Vector3[baseVertices4.Length];
       
        
        
    }

    // Update is called once per frame
    void Update()
    {
        DoTransform();
    }




    void DoTransform(){
        float angleRadians = Mathf.Atan2(displacement.z, displacement.x);

        float angle = angleRadians * Mathf.Rad2Deg-90;
        //create the matrices
        Matrix4x4 move= HW_Transforms.TranslationMat(displacement.x *Time.time , displacement.y *Time.time, displacement.z *Time.time);

        Matrix4x4 moveOrigin = HW_Transforms.TranslationMat(-displacement.x, -displacement.y, -displacement.z);
        
        Matrix4x4 moveObject = HW_Transforms.TranslationMat(displacement.x, displacement.y, displacement.z);
        
        Matrix4x4 rotate = HW_Transforms.RotateMat(angle , rotationAxis);

        //Se define el tamaño a las llantas a la medida del coche 
        Matrix4x4 scale = HW_Transforms.ScaleMat(0.4f, 0.4f, 0.4f);

        // Se asigna la posicion de la llanta 1 
        Matrix4x4 posWheel1 = HW_Transforms.TranslationMat(0.36f,2.45f,3.3f);

        // Se asigna la rotacion para que la llanta este de manera correcta
        Matrix4x4 rotWheel1 = HW_Transforms.RotateMat(-90f, AXIS.Y);

        // Se asigna la posicion de la llanta 2
        Matrix4x4 posWheel2 = HW_Transforms.TranslationMat(-1.5f,2.46f,0.42f);

        // Se asigna la rotacion para que la llanta este de manera correcta
        Matrix4x4 rotWheel2 = HW_Transforms.RotateMat(-90f, AXIS.Y);


        // Se asigna la posicion de la llanta 3
        Matrix4x4 posWheel3 = HW_Transforms.TranslationMat(-1.50f,2.45f,3.3f);

        // Se asigna la rotacion para que la llanta este de manera correcta
        Matrix4x4 rotWheel3 = HW_Transforms.RotateMat(-90f, AXIS.Y);

        // Se asigna la posicion de la llanta 4
        Matrix4x4 posWheel4 = HW_Transforms.TranslationMat(0.36f,2.45f,0.44f);

        // Se asigna la rotacion para que la llanta este de manera correcta
        Matrix4x4 rotWheel4 = HW_Transforms.RotateMat(-90f, AXIS.Y);


        // Se define la rotacion dentro del mismo eje de la llanta 
        // Para dar pespectiva de llanta en movimiento. 
        Matrix4x4 rotEje = HW_Transforms.RotateMat(-100 * Time.time, AXIS.X);



        //combine the matrices
        //operations are executed in backwards order
        Matrix4x4 composite = move * rotate;


        // Se hacen los ajustes para que las llantas sigan al coche y roten
        // de manera adecuada con el coche. 
        Matrix4x4 compositeWheel1 = posWheel1 * rotEje * rotWheel1 * scale;
        Matrix4x4 compositeWheel2 = posWheel2 * rotEje * rotWheel2 * scale;
        Matrix4x4 compositeWheel3 = posWheel3 * rotEje * rotWheel3 * scale;
        Matrix4x4 compositeWheel4 = posWheel4 * rotEje * rotWheel4 * scale;




        for (int i=0; i<baseVertices.Length; i++)
        {
            Vector4 temp = new Vector4(baseVertices[i].x, baseVertices[i].y, baseVertices[i].z, 1);

            newVertices[i] = composite * temp;
        }
        mesh.vertices = newVertices;
        mesh.RecalculateNormals();


        // Se recalculan las normales de la llanta 1 
        for (int i=0; i<baseVertices1.Length; i++)
        {
            Vector4 temp1 = new Vector4(baseVertices1[i].x, baseVertices1[i].y, baseVertices1[i].z, 1);

            newVertices1[i] = composite * compositeWheel1 * temp1;
            
        }
        wheel1mesh.vertices = newVertices1;
        wheel1mesh.RecalculateNormals();
        

        // Se recalculan las normales de la llanta 2
        for (int i=0; i<baseVertices2.Length; i++)
        {
            Vector4 temp2 = new Vector4(baseVertices2[i].x, baseVertices2[i].y, baseVertices2[i].z, 1);

            newVertices2[i] = composite *  compositeWheel2 * temp2;
        }
        wheel2mesh.vertices = newVertices2;
        wheel2mesh.RecalculateNormals();


        // Se recalculan las normales de la llanta 3        
        for (int i=0; i<baseVertices3.Length; i++)
        {
            Vector4 temp3 = new Vector4(baseVertices3[i].x, baseVertices3[i].y, baseVertices3[i].z, 1);

            newVertices3[i] = composite * compositeWheel3 *temp3;
        }
        wheel3mesh.vertices = newVertices3;
        wheel3mesh.RecalculateNormals();


        // Se recalculan las normales de la llanta 4
        for (int i=0; i<baseVertices4.Length; i++)
        {
            Vector4 temp4 = new Vector4(baseVertices4[i].x, baseVertices4[i].y, baseVertices4[i].z, 1);

            newVertices4[i] = composite * compositeWheel4 *temp4;
        }
        wheel4mesh.vertices = newVertices4;
        wheel4mesh.RecalculateNormals();

    
    }
}