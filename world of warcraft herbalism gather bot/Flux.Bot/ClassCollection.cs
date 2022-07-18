using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Flux.Bot
{
    /// <summary>
    /// This is a collection to hold class instances. It dynamically loads any classes found within DLLs
    /// in the base application directory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ClassCollection<T> : List<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassCollection&lt;T&gt;"/> class.
        /// </summary>
        public ClassCollection()
            : this(null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="path">The directory path to search for valid .NET assemblies.</param>
        /// <param name="loadRecursive">if set to <c>true</c> this class collection should recursively
        /// load dlls found in the current folder, and any subfolders.</param>
        public ClassCollection(string path, bool loadRecursive)
            : this(null, path, loadRecursive)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="ignoreAttribute">The type of an 'ignore' attribute to be used. If a type is decorated with
        /// this attribute, it will not be loaded into the collection.</param>
        public ClassCollection(Type ignoreAttribute)
            : this(ignoreAttribute, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="ignoreAttribute">The type of an 'ignore' attribute to be used. If a type is decorated with
        /// this attribute, it will not be loaded into the collection.</param>
        /// <param name="path">The directory path to search for valid .NET assemblies.</param>
        /// <param name="loadRecursive">>if set to <c>true</c> this class collection should recursively
        /// load dlls found in the current folder, and any subfolders.</param>
        public ClassCollection(Type ignoreAttribute, string path, bool loadRecursive)
        {
            IgnoreAttribute = ignoreAttribute;
            if (!string.IsNullOrEmpty(path))
            {
                var files = new List<string>();

                // Figure out which directory search we should do, this just makes life easier below.
                SearchOption option = loadRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                // Unfortunately; it requires 2 calls to do this. As the GetFiles method doesn't support
                // multiple search patterns.
                files.AddRange(Directory.GetFiles(path, "*.dll", option));
                //files.AddRange(Directory.GetFiles(path, "*.exe", option));

                foreach (string s in files)
                {
                    LoadClasses(s);
                }
            }
        }

        /// <summary>
        /// The type of ignore attribute being used with this collection.
        /// </summary>
        public Type IgnoreAttribute { get; private set; }

        /// <summary>
        /// Loads any classes found that are a descendant of T in the specified assembly path.
        /// </summary>
        /// <param name="assemblyPath">The DLL or EXE path.</param>
        /// <returns>The number of classes loaded.</returns>
        public int LoadClasses(string assemblyPath)
        {
            int ret = 0;

            // Make sure the path is valid.
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException("Could not find the specified file", assemblyPath);
            }

            try
            {
                Assembly asm = Assembly.LoadFrom(assemblyPath);
                Type[] types = asm.GetTypes();

                foreach (Type type in types)
                {
                    // Check to make sure the type is even a class
                    // and also a descendant of T
                    if (!type.IsClass || !type.IsSubclassOf(typeof(T)))
                    {
                        continue;
                    }
                    // If we set the ignore attribute, make sure we take that into account here.
                    if (IgnoreAttribute != null && type.GetCustomAttributes(IgnoreAttribute, true).Length != 0)
                    {
                        continue;
                    }

                    var loaded = (T)Activator.CreateInstance(type);

                    // Note: Contains uses the .Equals under the hood. It's best that you implement your own
                    // equality methods for this to have the best results.
                    if (Contains(loaded))
                    {
                        continue;
                    }
                    Add(loaded);
                    ret++;
                }
            }
            catch (BadImageFormatException)
            {
                // We eat this one on purpose. If the assembly isn't a valid .NET assembly, this exception is thrown.
                // Feel free to do extra work with it.
            }
            return ret;
        }
    }
}