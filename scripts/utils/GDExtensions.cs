using System.Linq;
using Godot;

public static class GDExtensions {
  public static T GetChildByType<T>(this Node node, bool recursive = true) {
    int childCount = node.GetChildCount();

    for (int i = 0; i < childCount; i++) {
      Node child = node.GetChild(i);
      if (child is T childT)
        return childT;

      if (recursive && child.GetChildCount() > 0) {
        T recursiveResult = child.GetChildByType<T>(true);
        if (recursiveResult != null)
          return recursiveResult;
      }
    }

    return default;
  }

  public static T GetParentByType<T>(this Node node) {
    Node parent = node.GetParent();
    if (parent != null) {
      if (parent is T parentT) {
        return parentT;
      } else {
        return parent.GetParentByType<T>();
      }
    }

    return default;
  }

  /// <summary>
  /// Gets the root scene node.
  /// </summary>
  public static T GetRootNode<T>(string rootNodeName) where T : Node {
    var sceneTree = Engine.GetMainLoop() as SceneTree;
    var sceneTreeRoot = sceneTree.Root;
    var rootNode = sceneTreeRoot.GetChildren().First(n => n.Name == rootNodeName) as T;
    return rootNode;
  }
}