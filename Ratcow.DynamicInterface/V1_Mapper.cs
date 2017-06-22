using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Ratcow.DynamicInterface
{
    /// <summary>
    /// A basic V1 implementation.
    /// </summary>
    public abstract class V1_Mapper : BaseMapper
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

            var setMethodIL = setMethod.GetILGenerator();
            setMethodIL.Emit(OpCodes.Ldarg_0);
            setMethodIL.Emit(OpCodes.Ldfld, field);
            setMethodIL.Emit(OpCodes.Ldarg_1);
            setMethodIL.Emit(OpCodes.Callvirt, instancePropertySetter);
            setMethodIL.Emit(OpCodes.Nop);
            setMethodIL.Emit(OpCodes.Ret);
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

            var getMethodIL = getMethod.GetILGenerator();
            getMethodIL.Emit(OpCodes.Ldarg_0);
            getMethodIL.Emit(OpCodes.Ldfld, field);
            getMethodIL.Emit(OpCodes.Callvirt, instancePropertyGetter);
            getMethodIL.Emit(OpCodes.Nop);
            getMethodIL.Emit(OpCodes.Ret);
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
                if (parameterInfo != null && parameterInfo.Length > 0)
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
                if (parameterInfo != null && parameterInfo.Length > 0)
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

            var methodIL = method.GetILGenerator();
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldfld, field);
            methodIL.Emit(OpCodes.Callvirt, instanceMethodInfo);
            methodIL.Emit(OpCodes.Ret);

        }

        /// <summary>
        /// 
        /// </summary>
        void AddMethod_value_noparams(TypeBuilder dynamicType, MethodInfo methodInfo, (string Name, string ImplementorName, object Implementor) instance, FieldBuilder field, ParameterInfo returnInfo)
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

            var methodIL = method.GetILGenerator();
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldfld, field);
            methodIL.Emit(OpCodes.Callvirt, instanceMethodInfo);
            methodIL.Emit(OpCodes.Ret);

        }

        /// <summary>
        /// 
        /// </summary>
        void AddMethod_void_params(TypeBuilder dynamicType, MethodInfo methodInfo, (string Name, string ImplementorName, object Implementor) instance, FieldBuilder field, ParameterInfo[] parameterInfo)
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
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual,
                null,
                paramTypes);

            var instanceMethodInfo = instance.Implementor.GetType().GetMethod(instance.ImplementorName);

            var methodIL = method.GetILGenerator();
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldfld, field);
            for (byte counter = 1; counter <= parameterInfo.Length; counter++)
            {
                switch (counter)
                {
                    case 1:
                        methodIL.Emit(OpCodes.Ldarg_1);
                        break;

                    case 2:
                        methodIL.Emit(OpCodes.Ldarg_2);
                        break;

                    case 3:
                        methodIL.Emit(OpCodes.Ldarg_3);
                        break;

                    default:
                        methodIL.Emit(OpCodes.Ldarg_S, counter);
                        break;
                }
            }
            methodIL.Emit(OpCodes.Callvirt, instanceMethodInfo);
            methodIL.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// 
        /// </summary>
        void AddMethod_value_params(TypeBuilder dynamicType, MethodInfo methodInfo, (string Name, string ImplementorName, object Implementor) instance, FieldBuilder field, ParameterInfo returnInfo, ParameterInfo[] parameterInfo)
        {
            var paramTypes = parameterInfo.Select(p => p.ParameterType).ToArray();

            var method = dynamicType.DefineMethod(
                $"{methodInfo.Name}",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual,
                returnInfo.ParameterType,
                null);

            var instanceMethodInfo = instance.Implementor.GetType().GetMethod(instance.ImplementorName);

            var methodIL = method.GetILGenerator();
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldfld, field);
            for (byte counter = 1; counter <= parameterInfo.Length; counter++)
            {
                switch (counter)
                {
                    case 1:
                        methodIL.Emit(OpCodes.Ldarg_1);
                        break;

                    case 2:
                        methodIL.Emit(OpCodes.Ldarg_2);
                        break;

                    case 3:
                        methodIL.Emit(OpCodes.Ldarg_3);
                        break;

                    default:
                        methodIL.Emit(OpCodes.Ldarg_S, counter);
                        break;
                }
            }
            methodIL.Emit(OpCodes.Callvirt, instanceMethodInfo);
            methodIL.Emit(OpCodes.Ret);
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

            var addMethodBuilderIL = addMethodBuilder.GetILGenerator();
            addMethodBuilderIL.Emit(OpCodes.Ldarg_0);
            addMethodBuilderIL.Emit(OpCodes.Ldfld, field);
            addMethodBuilderIL.Emit(OpCodes.Ldarg_1);
            addMethodBuilderIL.Emit(OpCodes.Callvirt, instanceEventAdder);
            addMethodBuilderIL.Emit(OpCodes.Nop);
            addMethodBuilderIL.Emit(OpCodes.Ret);

            eventBuilder.SetAddOnMethod(addMethodBuilder);


            var removeMethodBuilder = typeBuilder.DefineMethod(
                $"remove_{eventInfo.Name}",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName,
                null,
                new Type[] { eventInfo.EventHandlerType });

            removeMethodBuilder.SetImplementationFlags(MethodImplAttributes.Managed | MethodImplAttributes.Synchronized);

            var removeMethodBuilderIL = removeMethodBuilder.GetILGenerator();
            removeMethodBuilderIL.Emit(OpCodes.Ldarg_0);
            removeMethodBuilderIL.Emit(OpCodes.Ldfld, field);
            removeMethodBuilderIL.Emit(OpCodes.Ldarg_1);
            removeMethodBuilderIL.Emit(OpCodes.Callvirt, instanceEventRemover);
            removeMethodBuilderIL.Emit(OpCodes.Nop);
            removeMethodBuilderIL.Emit(OpCodes.Ret);

            eventBuilder.SetRemoveOnMethod(removeMethodBuilder);
        }
    }
}
