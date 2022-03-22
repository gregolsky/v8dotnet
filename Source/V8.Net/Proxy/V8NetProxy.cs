using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using V8.Net.Utils;

namespace V8.Net.Proxy
{
    public static unsafe partial class V8NetProxy
    {
        static V8NetProxy()
        {
            try
            {
                DynamicNativeLibraryResolver.Register(typeof(V8NetProxy).Assembly, V8NETPROXY);
            }
            catch (Exception ex)
            {
                var errString = $"{V8NETPROXY} version might be invalid, missing or not usable on current platform.";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    errString += " Initialization error could also be caused by missing 'Microsoft Visual C++ 2015 Redistributable Package' (or newer). It can be downloaded from https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads.";

                errString += $" Arch: {RuntimeInformation.OSArchitecture}, OSDesc: {RuntimeInformation.OSDescription}";

                throw new InvalidOperationException(errString, ex);
            }
        }

        private const string V8NETPROXY = "v8netproxy";

        [DllImport(V8NETPROXY, EntryPoint = "CreateV8EngineProxy")]
        public extern static NativeV8EngineProxy* CreateV8EngineProxy(bool enableDebugging, void* debugMessageDispatcher, int debugPort);

        [DllImport(V8NETPROXY, EntryPoint = "DestroyV8EngineProxy", ExactSpelling = false)]
        public static extern void DestroyV8EngineProxy(NativeV8EngineProxy* engine);

        [DllImport(V8NETPROXY, EntryPoint = "CreateContext")]
        public extern static NativeContext* CreateContext(NativeV8EngineProxy* engine, NativeObjectTemplateProxy* templatePoxy);

        [DllImport(V8NETPROXY, EntryPoint = "DeleteContext")]
        public extern static NativeContext* DeleteContext(NativeContext* context);

        [DllImport(V8NETPROXY, EntryPoint = "SetContext")]
        public extern static HandleProxy* SetContext(NativeV8EngineProxy* engine, NativeContext* context);

        [DllImport(V8NETPROXY, EntryPoint = "GetContext")]
        public extern static NativeContext* GetContext(NativeV8EngineProxy* engine);

        //  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  . 

        [DllImport(V8NETPROXY, EntryPoint = "SetFlagsFromString")]
        public static unsafe extern void SetFlagsFromString(NativeV8EngineProxy* engine, [MarshalAs(UnmanagedType.AnsiBStr)] string name);

        [DllImport(V8NETPROXY, EntryPoint = "RegisterGCCallback")]
        public static extern void RegisterGCCallback(NativeV8EngineProxy* engine, V8GarbageCollectionRequestCallback garbageCollectionRequestCallback);

        [DllImport(V8NETPROXY, EntryPoint = "ForceGC")]
        public static extern void ForceGC(NativeV8EngineProxy* engine);

        [DllImport(V8NETPROXY, EntryPoint = "DoIdleNotification")]
        public static extern bool DoIdleNotification(NativeV8EngineProxy* engine, int hint = 1000);

        [DllImport(V8NETPROXY, EntryPoint = "V8Execute", CharSet = CharSet.Unicode)]
        public static extern HandleProxy* V8Execute(NativeV8EngineProxy* engine, string script, string sourceName = null);

        [DllImport(V8NETPROXY, EntryPoint = "V8Compile", CharSet = CharSet.Unicode)]
        public static extern HandleProxy* V8Compile(NativeV8EngineProxy* engine, string script, string sourceName = null);

        [DllImport(V8NETPROXY, EntryPoint = "V8ExecuteCompiledScript")]
        public static extern HandleProxy* V8ExecuteCompiledScript(NativeV8EngineProxy* engine, HandleProxy* script);

        [DllImport(V8NETPROXY, EntryPoint = "TerminateExecution")]
        public static extern void TerminateExecution(NativeV8EngineProxy* engine);

        //  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  . 

        [DllImport(V8NETPROXY, EntryPoint = "CreateObjectTemplateProxy")]
        public static unsafe extern NativeObjectTemplateProxy* CreateObjectTemplateProxy(NativeV8EngineProxy* engine);

        // Return: NativeObjectTemplateProxy*

        /// <summary> Returns true if successful. False is returned if the engine is in the middle of running a script, or performing another request. </summary>
        [DllImport(V8NETPROXY, EntryPoint = "DeleteObjectTemplateProxy")]
        public static extern unsafe bool DeleteObjectTemplateProxy(NativeObjectTemplateProxy* objectTemplateProxy);


        // Return: HandleProxy*
        // (Note: returns a handle to the global object created by the context when the object template was set)

        [DllImport(V8NETPROXY, EntryPoint = "RegisterNamedPropertyHandlers")]
        public static extern void RegisterNamedPropertyHandlers(NativeObjectTemplateProxy* proxy,

            ManagedNamedPropertyGetter getter,
            ManagedNamedPropertySetter setter,
            ManagedNamedPropertyQuery query,
            ManagedNamedPropertyDeleter deleter,
            ManagedNamedPropertyEnumerator enumerator);

        [DllImport(V8NETPROXY, EntryPoint = "RegisterIndexedPropertyHandlers")]
        public static extern void RegisterIndexedPropertyHandlers(NativeObjectTemplateProxy* proxy,

            ManagedIndexedPropertyGetter getter,
            ManagedIndexedPropertySetter setter,
            ManagedIndexedPropertyQuery query,
            ManagedIndexedPropertyDeleter deleter,
            ManagedIndexedPropertyEnumerator enumerator);

        [DllImport(V8NETPROXY, EntryPoint = "UnregisterNamedPropertyHandlers")]
        public static extern void UnregisterNamedPropertyHandlers(NativeObjectTemplateProxy* proxy);


        [DllImport(V8NETPROXY, EntryPoint = "UnregisterIndexedPropertyHandlers")]
        public static extern void UnregisterIndexedPropertyHandlers(NativeObjectTemplateProxy* proxy);


        [DllImport(V8NETPROXY, EntryPoint = "SetCallAsFunctionHandler")]
        public static extern void SetCallAsFunctionHandler(NativeObjectTemplateProxy* proxy, NativeFunctionCallback callback);


        [DllImport(V8NETPROXY, EntryPoint = "CreateObjectFromTemplate")]
        public static unsafe extern HandleProxy* CreateObjectFromTemplate(NativeObjectTemplateProxy* objectTemplateProxy, Int32 objID);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "ConnectObject")]
        public static unsafe extern void ConnectObject(HandleProxy* handleProxy, Int32 objID, void* templateProxy = null);


        [DllImport(V8NETPROXY, EntryPoint = "GetObjectPrototype")]
        public static unsafe extern HandleProxy* GetObjectPrototype(HandleProxy* handleProxy);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "Call", CharSet = CharSet.Unicode)]
        /// <summary>
        /// Calls a property with a given name on a specified object as a function and returns the result.
        /// If the function name is null, then the subject is assumed to be a function object.
        /// </summary>
        public static unsafe extern HandleProxy* Call(HandleProxy* subject, string functionName, HandleProxy* _this, Int32 argCount, HandleProxy** args);
        // Return: HandleProxy*

        //  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  . 

        [DllImport(V8NETPROXY, EntryPoint = "SetObjectPropertyByName", CharSet = CharSet.Unicode)]
        public static unsafe extern bool SetObjectPropertyByName(HandleProxy* proxy, string name, HandleProxy* value, V8PropertyAttributes attributes = V8PropertyAttributes.None);


        [DllImport(V8NETPROXY, EntryPoint = "SetObjectPropertyByIndex")]
        public static unsafe extern bool SetObjectPropertyByIndex(HandleProxy* proxy, Int32 index, HandleProxy* value, V8PropertyAttributes attributes = V8PropertyAttributes.None);


        [DllImport(V8NETPROXY, EntryPoint = "GetObjectPropertyByName", CharSet = CharSet.Unicode)]
        public static unsafe extern HandleProxy* GetObjectPropertyByName(HandleProxy* proxy, string name);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "GetObjectPropertyByIndex")]
        public static unsafe extern HandleProxy* GetObjectPropertyByIndex(HandleProxy* proxy, Int32 index);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "DeleteObjectPropertyByName", CharSet = CharSet.Unicode)]
        public static unsafe extern bool DeleteObjectPropertyByName(HandleProxy* proxy, string name);


        [DllImport(V8NETPROXY, EntryPoint = "DeleteObjectPropertyByIndex")]
        public static unsafe extern bool DeleteObjectPropertyByIndex(HandleProxy* proxy, Int32 index);


        [DllImport(V8NETPROXY, EntryPoint = "SetObjectAccessor", CharSet = CharSet.Unicode)]
        public static unsafe extern void SetObjectAccessor(HandleProxy* proxy, Int32 managedObjectID, string name,

            NativeGetterAccessor getter, NativeSetterAccessor setter,
            V8AccessControl access, V8PropertyAttributes attributes);

        [DllImport(V8NETPROXY, EntryPoint = "SetObjectTemplateAccessor", CharSet = CharSet.Unicode)]
        public static unsafe extern void SetObjectTemplateAccessor(NativeObjectTemplateProxy* proxy, Int32 managedObjectID, string name,

            NativeGetterAccessor getter, NativeSetterAccessor setter,
            V8AccessControl access, V8PropertyAttributes attributes);

        [DllImport(V8NETPROXY, EntryPoint = "SetObjectTemplateProperty", CharSet = CharSet.Unicode)]
        public static unsafe extern void SetObjectTemplateProperty(NativeObjectTemplateProxy* proxy, string name, HandleProxy* value, V8PropertyAttributes attributes = V8PropertyAttributes.None);


        [DllImport(V8NETPROXY, EntryPoint = "GetPropertyNames")]
        public static unsafe extern HandleProxy* GetPropertyNames(HandleProxy* proxy);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "GetOwnPropertyNames")]
        public static unsafe extern HandleProxy* GetOwnPropertyNames(HandleProxy* proxy);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "GetPropertyAttributes", CharSet = CharSet.Unicode)]
        public static unsafe extern V8PropertyAttributes GetPropertyAttributes(HandleProxy* proxy, string name);


        [DllImport(V8NETPROXY, EntryPoint = "GetArrayLength")]
        public static unsafe extern Int32 GetArrayLength(HandleProxy* proxy);


        //  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  . 

        [DllImport(V8NETPROXY, EntryPoint = "CreateFunctionTemplateProxy", CharSet = CharSet.Unicode)]
        public static unsafe extern NativeFunctionTemplateProxy* CreateFunctionTemplateProxy(NativeV8EngineProxy* engine, string className, NativeFunctionCallback callback);

        /// <summary> Returns true if successful. False is returned if the engine is in the middle of running a script, or performing another request. </summary>
        [DllImport(V8NETPROXY, EntryPoint = "DeleteFunctionTemplateProxy")]

        public static extern unsafe bool DeleteFunctionTemplateProxy(NativeFunctionTemplateProxy* functionTemplateProxy);


        [DllImport(V8NETPROXY, EntryPoint = "GetFunctionInstanceTemplateProxy")]
        public static unsafe extern NativeObjectTemplateProxy* GetFunctionInstanceTemplateProxy(NativeFunctionTemplateProxy* functionTemplateProxy);

        // Return: NativeObjectTemplateProxy*

        [DllImport(V8NETPROXY, EntryPoint = "GetFunctionPrototypeTemplateProxy")]
        public static unsafe extern NativeObjectTemplateProxy* GetFunctionPrototypeTemplateProxy(NativeFunctionTemplateProxy* functionTemplateProxy);

        // Return: NativeObjectTemplateProxy*

        [DllImport(V8NETPROXY, EntryPoint = "GetFunction")]
        public static unsafe extern HandleProxy* GetFunction(NativeFunctionTemplateProxy* functionTemplateProxy);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "CreateInstanceFromFunctionTemplate")]
        public static unsafe extern HandleProxy* CreateInstanceFromFunctionTemplate(NativeFunctionTemplateProxy* functionTemplateProxy, Int32 objID, Int32 argCount = 0, HandleProxy** args = null);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "SetFunctionTemplateProperty", CharSet = CharSet.Unicode)]
        public static unsafe extern void SetFunctionTemplateProperty(NativeFunctionTemplateProxy* proxy, string name, HandleProxy* value, V8PropertyAttributes attributes = V8PropertyAttributes.None);



        //  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  . 

        [DllImport(V8NETPROXY, EntryPoint = "CreateBoolean")]
        public static extern HandleProxy* CreateBoolean(NativeV8EngineProxy* engine, bool b);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "CreateInteger")]
        public static extern HandleProxy* CreateInteger(NativeV8EngineProxy* engine, Int32 num);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "CreateNumber")]
        public static extern HandleProxy* CreateNumber(NativeV8EngineProxy* engine, double num);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "CreateString", CharSet = CharSet.Unicode)]
        public static extern HandleProxy* CreateString(NativeV8EngineProxy* engine, string str);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "CreateError", CharSet = CharSet.Unicode)]
        public static extern HandleProxy* CreateError(NativeV8EngineProxy* engine, string message, JSValueType errorType);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "CreateDate")]
        public static extern HandleProxy* CreateDate(NativeV8EngineProxy* engine, double ms);

        // Return: HandleProxy*

        [DllImport(V8NETPROXY, EntryPoint = "CreateObject")]
        public static extern HandleProxy* CreateObject(NativeV8EngineProxy* engine, Int32 managedObjectID);


        [DllImport(V8NETPROXY, EntryPoint = "CreateArray")]
        public static extern HandleProxy* CreateArray(NativeV8EngineProxy* engine, HandleProxy** items = null, Int32 length = 0);


        [DllImport(V8NETPROXY, EntryPoint = "CreateStringArray", CharSet = CharSet.Unicode)]
        public static extern HandleProxy* CreateStringArray(NativeV8EngineProxy* engine, char** items, Int32 length = 0);


        [DllImport(V8NETPROXY, EntryPoint = "CreateNullValue", CharSet = CharSet.Unicode)]
        public static extern HandleProxy* CreateNullValue(NativeV8EngineProxy* engine);


        //  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  . 

        [DllImport(V8NETPROXY, EntryPoint = "MakeWeakHandle")]
        public static extern void MakeWeakHandle(HandleProxy* handleProxy);


        [DllImport(V8NETPROXY, EntryPoint = "MakeStrongHandle")]
        public static extern void MakeStrongHandle(HandleProxy* handleProxy);


        [DllImport(V8NETPROXY, EntryPoint = "DisposeHandleProxy")]
        public static extern void DisposeHandleProxy(HandleProxy* handle);

        // (required for disposing of the associated V8 handle marshalled in "_HandleProxy")

        [DllImport(V8NETPROXY, EntryPoint = "UpdateHandleValue")]
        public static extern void UpdateHandleValue(HandleProxy* handle);


        [DllImport(V8NETPROXY, EntryPoint = "GetHandleManagedObjectID")]
        public static extern int GetHandleManagedObjectID(HandleProxy* handle);


        // --------------------------------------------------------------------------------------------------------------------
        // Tests

        [DllImport(V8NETPROXY, EntryPoint = "CreateHandleProxyTest")]
        public static extern HandleProxy* CreateHandleProxyTest();


        [DllImport(V8NETPROXY, EntryPoint = "CreateV8EngineProxyTest")]
        public static extern NativeV8EngineProxy* CreateV8EngineProxyTest();


        [DllImport(V8NETPROXY, EntryPoint = "CreateObjectTemplateProxyTest")]
        public static extern NativeObjectTemplateProxy* CreateObjectTemplateProxyTest();


        [DllImport(V8NETPROXY, EntryPoint = "CreateFunctionTemplateProxyTest")]
        public static extern NativeFunctionTemplateProxy* CreateFunctionTemplateProxyTest();


        [DllImport(V8NETPROXY, EntryPoint = "DeleteTestData")]
        public static extern void DeleteTestData(void* data);


    }
}
