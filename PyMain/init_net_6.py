from pythonnet import set_runtime, _RUNTIME

if _RUNTIME is None:
    from clr_loader import get_coreclr

    runtime = get_coreclr("runtimeconfig.json")
    set_runtime(runtime)

    import clr
    clr.AddReference("Fasterflect")
