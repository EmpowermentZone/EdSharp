from distutils.core import setup
import py2exe

setup(
    options = {"py2exe": {"compressed": 1,
                          "optimize": 2,
                          "bundle_files": 2}},
    zipfile = None,
    version = "1.1",
    description = "Interactive Python environment and script runner",
    name = "InPy",

    windows = ["InPy.py"],
    console = ["InPyC.py"],
    )
