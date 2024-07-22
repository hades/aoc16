FROM archlinux:latest

RUN pacman -Suy -q --noconfirm

RUN pacman -S -q --noconfirm dotnet-sdk

ARG USERNAME=hades
ARG USER_UID=1000
ARG USER_GID=$USER_UID

RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME

USER $USERNAME
