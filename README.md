# Directory Manager

Directory Manager is a simple Plugin for adding more functionality to the DirectoryInfo Object under the System.IO namespace, our plugin lives under the System.IO.Expand namespace.

## How To Use:

to install the plugin run this command in the Package Manager Console :  `PM> Install-Package DirectoryManager` or Browse for DirectoryManager in the search area in the NuGet Package Manager.  
to start using the plugin add a reference to the plugin by writing :   `using System.IO.Expand;`  

now you have access to our extension methods for the DirectoryInfo object.  

here is a list of the plugins that you have access to:
- Compare()
- CompareAsync()
- MoveToDesktop() 
- Rename()
- GenerateRandomName()
- Copy()
- CopyToDesktop()
- Search()
- SearchAsync()
- GetTotalSize()
- GetTotalSizeAsync()
- LaunchFolderView()

=> the Compare Method is used to compare the current DirectoryInfo instant with the given Directory info, you can specified 
the output from two option `OutputOptions.Matching` to get the matching results, and `OutputOptions.NonMatching` 
to get non matching results, you can also provide a CompareOption to compare with Name, FullName, DateCreation, or TotalSize
Note: when we compare the two folders the files are ignored, we only compare the folders at the root

=> the Move method is used to move a folder to a new directory by keeping the same name of the directory,
for example let say that you have a folder at \Desktop called Folder1 `(path : C:\...\Desktop\Folder1)` and you want to move 
it to `C:\...\Documents`, so now you just need to specify the destination path, like so `Move("C:\...\Documents")` and the folder 
will be in the new location at `C:\...\Documents\Folder1`, so the main deference here to the `MoveTo()` methods 
is that we keep the folder name, but if yo used the `MoveTo("C:\...\Documents")` method you will see the content of your 
Folder1 being moved to \Document, so here they treat Documents as the new name of the folder.

=> the Rename Method is used to rename your folder all you have to do is to pass your new folder name to the method 
like so : `Rename("that my new name")` and the new name will be assigned to folder, keeping the folder at his location.

=> the Copy Method work the same as Move(), the only deference is that the Copy method just make a copy of your folder to the specified location keeping the folder with the same name

=> the Search Method it used to search inside a folder without going inside the subdirectories it only looks at the root of the specified folder, and give you the ability to search with regex and with strings.

=> the GetTotalSize() get you the total size of a folder.

=> the LaunchFolderView() open the current folder in the file explorer

also you will find a DirectoryManager Class that will allow you to create a DirectoryManager Object instant, the DirectoryManager is just a layer on top of 
DirectoryInfo, for that this will be a valid syntax:
```
var testFolderPath = Path.Combine(DirectoryManager.GetDesktopPath(), "TestFolder");

DirectoryManager manager = new DirectoryManager(testFolderPath, true);
DirectoryInfo dir = new DirectoryInfo(testFolderPath);

manager = dir;
//or
dir = manager;
```
to create an instant of DirectoryManager class you have to specified a valid path and the path should point to an existing Directory or you will get a DirectoryNotFoundException, if you want to create a if not exist add true to the constructor like you see in the example above, this will allow us to create the directory if it not exist.

the DirectoryManger class give you access to all methods in the DirectoryInfo class, and add on top of it more functionality.
the class give you an event to watch the changes on your directory so you get notify with all changes on your directory, like if a sub folder has been add or if deleted and even if has been renamed, all you have to do is to "enable ContentChangedWatcher" and subscribe to "DirectoryContentChanged" event:

```
static void Main()
{
    var testFolderPath = Path.Combine(DirectoryManager.GetDesktopPath(), "TestFolder");
    DirectoryManager manager = new DirectoryManager(testFolderPath, true);

    manager.EnableContentChangedWatcher = true;
    manager.DirectoryContentChanged += DirectoryContentChanged;
}

private void DirectoryContentChanged(object sender, DirectoryContentChangedEventArgs e)
{
    Console.WriteLine($"path = {e.FullPath} , Change Type : {e.ChangeType}");
}

```

also the DirectoryManager class has some static methods that you can use to move, copy or rename a list of directories.
we have built a simple program that you can explore to see deferent implementation of the plugin.
