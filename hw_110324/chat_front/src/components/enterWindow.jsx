export const EnterWindow = ({
  accessToken,
  authorize,
  joinChat,
  setUserName,
}) => {
  return (
    <div className="window">
      <label>
        <div>Your name:</div>
        <input
          type="text"
          name="username"
          onChange={(e) => setUserName(e.target.value)}
        />
      </label>
      <div>
        {!accessToken && <button onClick={authorize}>Authorize</button>}
        {accessToken && <button onClick={joinChat}>Join chat</button>}
      </div>
    </div>
  );
};
