import functools
from typing import *


def flask_warning_suppressor(func: Callable[..., Any]) -> Callable[..., Any]:
    @functools.wraps(func)
    def wrapper(*args: Tuple[Any, ...], **kwargs: Dict[str, Any]) -> Any:
        if args and isinstance(args[0], str) and args[0].startswith('WARNING: This is a development server.'):
            return ''
        return func(*args, **kwargs)

    return wrapper