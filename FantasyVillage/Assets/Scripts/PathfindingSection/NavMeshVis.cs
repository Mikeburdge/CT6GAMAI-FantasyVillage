using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

// Obj exporter component based on: http://wiki.unity3d.com/index.php?title=ObjExporter
 
namespace PathfindingSection
{
    public class NavMeshVis : MonoBehaviour {
        private void Start()    
        {
            //Export();
        }
        static void Export() {
            var triangulatedNavMesh = NavMesh.CalculateTriangulation();

            var mesh = new Mesh
            {
                name = "ExportedNavMesh",
                vertices = triangulatedNavMesh.vertices,
                triangles = triangulatedNavMesh.indices
            };
            var filename = Application.dataPath + "/Prefabs/NavMeshObject/Exported NavMesh.obj";
            MeshToFile(mesh, filename);
            print("NavMesh exported as '" + filename + "'");
        }


        static string MeshToString(Mesh mesh) {
            var sb = new StringBuilder();
 
            sb.Append("g ").Append(mesh.name).Append("\n");
            foreach (var v in mesh.vertices) {
                sb.Append($"v {v.x} {v.y} {v.z}\n");
            }
            sb.Append("\n");
            foreach (var v in mesh.normals) {
                sb.Append($"vn {v.x} {v.y} {v.z}\n");
            }
            sb.Append("\n");
            foreach (Vector3 v in mesh.uv) {
                sb.Append($"vt {v.x} {v.y}\n");
            }
            for (var material = 0; material < mesh.subMeshCount; material++) {
                sb.Append("\n");
                //sb.Append("usemtl ").Append(mats[material].name).Append("\n");
                //sb.Append("usemap ").Append(mats[material].name).Append("\n");
 
                var triangles = mesh.GetTriangles(material);
                for (var i=0;i<triangles.Length;i+=3) {
                    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[i]+1, triangles[i+1]+1, triangles[i+2]+1));
                }
            }
            return sb.ToString();
        }
 
        static void MeshToFile(Mesh mesh, string filename) {
            using (var sw = new StreamWriter(filename)) {
                sw.Write(MeshToString(mesh));
            }
        }
    }
}
 