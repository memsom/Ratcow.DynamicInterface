namespace Ratcow.DynamicInterface;

/// <summary>
/// A basic V1 implementation.
/// </summary>
public abstract class V1Mapper : BaseMapper
{
    /// <summary>
    ///
    /// </summary>
    protected override void AddConstructor(TypeBuilder typeBuilder, FieldBuilder[] fields)
    {
        var paramList = fields.Select(f => f.FieldType).ToArray();
        var type = Type.GetType("System.Object");
        var ctor = type.GetConstructor(new Type[0]);

        var constructorBuilder = typeBuilder.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.Standard,
            paramList);
        var ctorIL = constructorBuilder.GetILGenerator();

        ctorIL.Emit(OpCodes.Ldarg_0);
        ctorIL.Emit(OpCodes.Call, ctor);
        byte counter = 1;
        foreach (var field in fields)
        {
            ctorIL.Emit(OpCodes.Ldarg_0);

            switch (counter)
            {
                case 1:
                    ctorIL.Emit(OpCodes.Ldarg_1);
                    break;

                case 2:
                    ctorIL.Emit(OpCodes.Ldarg_2);
                    break;

                case 3:
                    ctorIL.Emit(OpCodes.Ldarg_3);
                    break;

                default:
                    ctorIL.Emit(OpCodes.Ldarg_S, counter);
                    break;
            }

            counter++;


            ctorIL.Emit(OpCodes.Stfld, field);
        }

        ctorIL.Emit(OpCodes.Ret);
    }

    /// <summary>
    /// Generate fields for the contained data
    /// </summary>
    protected override FieldBuilder[] AddFields(TypeBuilder typeBuilder, object[] instances)
    {
        var result = new List<FieldBuilder>();
        foreach (var instance in instances)
        {
            var type = instance.GetType();
            var field = typeBuilder.DefineField(type.Name.ToLower(), type, FieldAttributes.Public);
            result.Add(field);
        }

        return result.ToArray();
    }

    protected override void AddProperty(TypeBuilder typeBuilder, PropertyInfo propertyInfo, (string Name, string InstanceName, object Implementor) instance, FieldBuilder field)
    {
        var propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.None, propertyInfo.PropertyType, null);

        //get the property info for the property
        var instancePropertyInfo = instance.Implementor.GetType().GetProperty(instance.InstanceName);
        var instancePropertyGetter = instancePropertyInfo.GetGetMethod();
        var instancePropertySetter = instancePropertyInfo.GetSetMethod();

        var getMethod = AddPropertyGetter(typeBuilder, propertyInfo, field, instancePropertyGetter);
        var setMethod = AddPropertySetter(typeBuilder, propertyInfo, field, instancePropertySetter);

        propertyBuilder.SetGetMethod(getMethod);
        propertyBuilder.SetSetMethod(setMethod);
    }

    /// <summary>
    /// Creates a generic getter for the contained instances property
    /// </summary>
    MethodBuilder AddPropertySetter(TypeBuilder typeBuilder, PropertyInfo propertyInfo, FieldBuilder field, MethodInfo instancePropertySetter)
    {
        var setMethod = typeBuilder.DefineMethod(
            $"set_{propertyInfo.Name}",
            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual,
            null,
            new Type[] { propertyInfo.PropertyType });

        var setMethodIl = setMethod.GetILGenerator();
        setMethodIl.Emit(OpCodes.Ldarg_0);
        setMethodIl.Emit(OpCodes.Ldfld, field);
        setMethodIl.Emit(OpCodes.Ldarg_1);
        setMethodIl.Emit(OpCodes.Callvirt, instancePropertySetter);
        setMethodIl.Emit(OpCodes.Nop);
        setMethodIl.Emit(OpCodes.Ret);
        return setMethod;
    }

    /// <summary>
    /// Creates a generic setter for the contained instances property
    /// </summary>
    MethodBuilder AddPropertyGetter(TypeBuilder typeBuilder, PropertyInfo propertyInfo, FieldBuilder field, MethodInfo instancePropertyGetter)
    {
        var getMethod = typeBuilder.DefineMethod(
            $"get_{propertyInfo.Name}",
            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual,
            propertyInfo.PropertyType,
            null);

        var getMethodIl = getMethod.GetILGenerator();
        getMethodIl.Emit(OpCodes.Ldarg_0);
        getMethodIl.Emit(OpCodes.Ldfld, field);
        getMethodIl.Emit(OpCodes.Callvirt, instancePropertyGetter);
        getMethodIl.Emit(OpCodes.Nop);
        getMethodIl.Emit(OpCodes.Ret);
        return getMethod;
    }

    /// <summary>
    /// Generate methods for the contained instances
    /// </summary>
    protected override void AddMethod(TypeBuilder dynamicType, MethodInfo method, (string Name, string implementorName, object Implementor) methodInstance, FieldBuilder field)
    {
        var returnInfo = method.ReturnParameter;
        var parameterInfo = method.GetParameters();

        if (returnInfo != null && returnInfo.ParameterType != typeof(void))
        {
            if (parameterInfo is { Length: > 0 })
            {
                //at least one param
                AddMethod_value_params(dynamicType, method, methodInstance, field, returnInfo, parameterInfo);
            }
            else
            {
                //no params
                AddMethod_value_noparams(dynamicType, method, methodInstance, field, returnInfo);
            }
        }
        else
        {
            //void method
            if (parameterInfo is { Length: > 0 })
            {
                //at least one param
                AddMethod_void_params(dynamicType, method, methodInstance, field, parameterInfo);
            }
            else
            {
                //no params
                AddMethod_void_noparams(dynamicType, method, methodInstance, field);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    void AddMethod_void_noparams(TypeBuilder dynamicType, MethodInfo methodInfo, (string Name, string ImplementorName, object Implementor) instance, FieldBuilder field)
    {
        var method = dynamicType.DefineMethod(
            $"{methodInfo.Name}",
            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual,
            null,
            null);

        var instanceMethodInfo = instance.Implementor.GetType().GetMethod(instance.ImplementorName);

        var methodIl = method.GetILGenerator();
        methodIl.Emit(OpCodes.Ldarg_0);
        methodIl.Emit(OpCodes.Ldfld, field);
        methodIl.Emit(OpCodes.Callvirt, instanceMethodInfo);
        methodIl.Emit(OpCodes.Ret);
    }

    /// <summary>
    ///
    /// </summary>
    void AddMethod_value_noparams(TypeBuilder dynamicType, MethodInfo methodInfo, (string Name, string ImplementorName, object Implementor) instance, FieldBuilder field,
        ParameterInfo returnInfo)
    {
        //IL_0001: ldarg.0
        //IL_0002: ldfld class ModelTestImpl.SimpleMethodTest ModelTestImpl.SimpleMethodTestImpl::simplemethodtest
        //IL_0007: callvirt instance int32 ModelTestImpl.SimpleMethodTest::GetInt()
        //IL_0010: ret

        var method = dynamicType.DefineMethod(
            $"{methodInfo.Name}",
            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual,
            returnInfo.ParameterType,
            null);

        var instanceMethodInfo = instance.Implementor.GetType().GetMethod(instance.ImplementorName);

        var methodIl = method.GetILGenerator();
        methodIl.Emit(OpCodes.Ldarg_0);
        methodIl.Emit(OpCodes.Ldfld, field);
        methodIl.Emit(OpCodes.Callvirt, instanceMethodInfo);
        methodIl.Emit(OpCodes.Ret);
    }

    /// <summary>
    ///
    /// </summary>
    void AddMethod_void_params(
        TypeBuilder dynamicType,
        MethodInfo methodInfo,
        (string Name, string ImplementorName, object Implementor) instance,
        FieldBuilder field,
        ParameterInfo[] parameterInfo)
    {
        //IL_0000: nop
        //IL_0001: ldarg.0
        //IL_0002: ldfld class ModelTestImpl.SimpleMethodTest ModelTestImpl.SimpleMethodTestImpl::simplemethodtest
        //IL_0007: ldarg.1
        //IL_0008: callvirt instance void ModelTestImpl.SimpleMethodTest::SetInt(int32)
        //IL_000d: nop
        //IL_000e: ret

        var paramTypes = parameterInfo.Select(p => p.ParameterType).ToArray();

        var method = dynamicType.DefineMethod(
            $"{methodInfo.Name}",
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Final,
            null,
            paramTypes);

        var instanceMethodInfo = instance.Implementor.GetType().GetMethod(instance.ImplementorName);

        var methodIl = method.GetILGenerator();
        methodIl.Emit(OpCodes.Ldarg_0);
        methodIl.Emit(OpCodes.Ldfld, field);
        for (byte counter = 1; counter <= parameterInfo.Length; counter++)
        {
            switch (counter)
            {
                case 1:
                    methodIl.Emit(OpCodes.Ldarg_1);
                    break;

                case 2:
                    methodIl.Emit(OpCodes.Ldarg_2);
                    break;

                case 3:
                    methodIl.Emit(OpCodes.Ldarg_3);
                    break;

                default:
                    methodIl.Emit(OpCodes.Ldarg_S, counter);
                    break;
            }
        }

        methodIl.Emit(OpCodes.Callvirt, instanceMethodInfo);
        methodIl.Emit(OpCodes.Ret);
    }

    /// <summary>
    ///
    /// </summary>
    void AddMethod_value_params(
        TypeBuilder dynamicType,
        MethodInfo methodInfo,
        (string Name, string ImplementorName, object Implementor) instance,
        FieldBuilder field,
        ParameterInfo returnInfo, ParameterInfo[] parameterInfo)
    {
        var method = dynamicType.DefineMethod(
            $"{methodInfo.Name}",
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Final,
            returnInfo.ParameterType,
            null);

        var instanceMethodInfo = instance.Implementor?.GetType()?.GetMethod(instance.ImplementorName);

        var methodIl = method.GetILGenerator();
        methodIl.Emit(OpCodes.Ldarg_0);
        methodIl.Emit(OpCodes.Ldfld, field);
        for (byte counter = 1; counter <= parameterInfo.Length; counter++)
        {
            switch (counter)
            {
                case 1:
                    methodIl.Emit(OpCodes.Ldarg_1);
                    break;

                case 2:
                    methodIl.Emit(OpCodes.Ldarg_2);
                    break;

                case 3:
                    methodIl.Emit(OpCodes.Ldarg_3);
                    break;

                default:
                    methodIl.Emit(OpCodes.Ldarg_S, counter);
                    break;
            }
        }

        if (instanceMethodInfo is not null)
        {
            methodIl.Emit(OpCodes.Callvirt, instanceMethodInfo);
        }

        methodIl.Emit(OpCodes.Ret);
    }

    /// <summary>
    ///
    /// </summary>
    protected override void AddEvent(TypeBuilder typeBuilder, EventInfo eventInfo, (string Name, string InstanceName, object Implementor) instance, FieldBuilder field)
    {
        var eventBuilder = typeBuilder.DefineEvent(eventInfo.Name, EventAttributes.None, eventInfo.EventHandlerType);

        //get the event info for the event
        var instanceEventInfo = instance.Implementor.GetType().GetEvent(instance.InstanceName);
        var instanceEventAdder = instanceEventInfo.GetAddMethod();
        var instanceEventRemover = instanceEventInfo.GetRemoveMethod();

        var addMethodBuilder = typeBuilder.DefineMethod(
            $"add_{eventInfo.Name}",
            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName,
            null,
            new Type[] { eventInfo.EventHandlerType });

        addMethodBuilder.SetImplementationFlags(MethodImplAttributes.Managed | MethodImplAttributes.Synchronized);

        var addMethodBuilderIl = addMethodBuilder.GetILGenerator();
        addMethodBuilderIl.Emit(OpCodes.Ldarg_0);
        addMethodBuilderIl.Emit(OpCodes.Ldfld, field);
        addMethodBuilderIl.Emit(OpCodes.Ldarg_1);
        addMethodBuilderIl.Emit(OpCodes.Callvirt, instanceEventAdder);
        addMethodBuilderIl.Emit(OpCodes.Nop);
        addMethodBuilderIl.Emit(OpCodes.Ret);

        eventBuilder.SetAddOnMethod(addMethodBuilder);


        var removeMethodBuilder = typeBuilder.DefineMethod(
            $"remove_{eventInfo.Name}",
            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName,
            null,
            new Type[] { eventInfo.EventHandlerType });

        removeMethodBuilder.SetImplementationFlags(MethodImplAttributes.Managed | MethodImplAttributes.Synchronized);

        var removeMethodBuilderIl = removeMethodBuilder.GetILGenerator();
        removeMethodBuilderIl.Emit(OpCodes.Ldarg_0);
        removeMethodBuilderIl.Emit(OpCodes.Ldfld, field);
        removeMethodBuilderIl.Emit(OpCodes.Ldarg_1);
        removeMethodBuilderIl.Emit(OpCodes.Callvirt, instanceEventRemover);
        removeMethodBuilderIl.Emit(OpCodes.Nop);
        removeMethodBuilderIl.Emit(OpCodes.Ret);

        eventBuilder.SetRemoveOnMethod(removeMethodBuilder);
    }
}